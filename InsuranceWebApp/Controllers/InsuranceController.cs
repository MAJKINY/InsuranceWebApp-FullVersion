using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InsuranceWebApp.Data;
using InsuranceWebApp.Models;
using InsuranceWebApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace InsuranceWebApp.Controllers
{
    
    public class InsuranceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InsuranceController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = UserRoles.Admin)]
        // GET: Insurances
        public async Task<IActionResult> Index()
        {
            return View(await _context.Insurance.ToListAsync());
        }

        [Authorize(Roles = UserRoles.Admin)]
        // GET: Insurances/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insurance = await _context.Insurance.FirstOrDefaultAsync(m => m.Id == id);
            if (insurance == null)
            {
                return NotFound();
            }

            var client = _context.Clients.Find(insurance.ClientId);
            if (client == null)
            {
                return NotFound();
            }

            var viewModel = new CreateInsuranceViewModel
            {
                InsuranceData = insurance,
                ClientData = client
            };

            return View(viewModel);
        }

        [Authorize(Roles = UserRoles.Admin)]
        // GET: Policyholders/Create
        public async Task<IActionResult> Create(int clientId)
        {
            var client = await _context.Clients.FindAsync(clientId);
            if(client == null)
            {
                return NotFound();
            }

            var viewModel = new CreateInsuranceViewModel
            {
                InsuranceData = new Insurance
                {
                    ClientId = clientId
                },
                ClientData = client
            };

            return View(viewModel);
        }

        // POST: Policyholders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateInsuranceViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                foreach (var error in errors)
                {
                    Console.WriteLine("ModelState error: " + error);
                }

                viewModel.ClientData = await _context.Clients.FindAsync(viewModel.InsuranceData.ClientId);
                return View(viewModel);
            }

            Console.WriteLine(ModelState.IsValid.ToString());

            try
            {
                _context.Add(viewModel.InsuranceData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            try
            {
            await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return RedirectToAction("Details", "Admin", new { id = viewModel.InsuranceData.ClientId });
        }

        // GET: Insurances/Edit/5
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insurance = await _context.Insurance.FindAsync(id);
            if (insurance == null)
            {
                return NotFound();
            }

            var client = _context.Clients.Find(insurance.ClientId);
            if(client == null)
            {
                return NotFound();
            }

            var viewModel = new CreateInsuranceViewModel
            {
                InsuranceData = insurance,
                ClientData = client
            };

            return View(viewModel);
        }

        // POST: Insurances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CreateInsuranceViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var existingClient = _context.Clients
                .Include(c => c.Insurance)
                .FirstOrDefault(c => c.Id == viewModel.ClientData.Id);

            if (existingClient == null)
            {
                return NotFound();
            }

            var existingInsurance = existingClient.Insurance
                .FirstOrDefault(i => i.Id == viewModel.InsuranceData.Id);

            if (existingInsurance == null)
            {
                return NotFound();
            }

            existingInsurance.InsuranceType = viewModel.InsuranceData.InsuranceType;
            existingInsurance.Amount = viewModel.InsuranceData.Amount;
            existingInsurance.SubjectOfInsurance = viewModel.InsuranceData.SubjectOfInsurance;
            existingInsurance.StartOfInsurance = viewModel.InsuranceData.StartOfInsurance;
            existingInsurance.EndOfInsurance = viewModel.InsuranceData.EndOfInsurance;

            _context.SaveChanges();

            return RedirectToAction("Details", "Admin", new { id = viewModel.InsuranceData.ClientId });
        }

        // GET: Insurances/Delete/5
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insurance = await _context.Insurance
                .FirstOrDefaultAsync(m => m.Id == id);
            if (insurance == null)
            {
                return NotFound();
            }

            return View(insurance);
        }

        // POST: Insurances/Delete/5
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var insurance = await _context.Insurance.FindAsync(id);
            if (insurance == null)
            {
                return NotFound();
            }

            var clientId = insurance.ClientId;

            _context.Insurance.Remove(insurance);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Admin", new { id = clientId });
        }

        [Authorize(Roles = UserRoles.Client)]
        public async Task<IActionResult> ClientInsuranceDetails(int? id)
        {
            if (id == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var client = await _context.Clients.FirstOrDefaultAsync(c => c.IdentityUserId == userId);
            if (client == null)
                return NotFound();

            var insurance = await _context.Insurance.FirstOrDefaultAsync(i => i.Id == id && i.ClientId == client.Id);
            if (insurance == null)
                return NotFound();

            var viewModel = new CreateInsuranceViewModel
            {
                InsuranceData = insurance,
                ClientData = client
            };

            return View("Details", viewModel);
        }

        private bool InsuranceExists(int id)
        {
            return _context.Insurance.Any(e => e.Id == id);
        }
    }
}
