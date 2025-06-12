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
using InsuranceWebApp.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace InsuranceWebApp.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IClientService _clientService;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(ApplicationDbContext context, IClientService clientService, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _clientService = clientService;
            _userManager = userManager;
        }

        // GET: Policyholders
        public async Task<IActionResult> Index(string searchString, int page = 1)
        {
            int totalClients;
            int pageSize = 10;

            var query = _context.Clients.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                var terms = searchString.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (terms.Length == 1)
                {
                    query = query.Where(c =>
                        c.FirstName.Contains(terms[0]) ||
                        c.LastName.Contains(terms[0]));
                }
                else if (terms.Length >= 2)
                {
                    string first = terms[0];
                    string last = string.Join(" ", terms.Skip(1));
                    query = query.Where(c =>
                        c.FirstName.Contains(first) &&
                        c.LastName.Contains(last));
                }

                totalClients = await query.CountAsync();
            }
            else
            {
                totalClients = await _context.Clients.CountAsync();
            }


            var clients = await query
                    .OrderBy(c => c.LastName)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

            var viewModel = new ClientListViewModel
            {
                Clients = clients,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalClients / (double)pageSize),
                SearchString = searchString
            };

            return View(viewModel);
        }

        // GET: Policyholders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var policyholder = await _context.Clients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (policyholder == null)
            {
                return NotFound();
            }

            var insurances = await _context.Insurance.Where(i => i.ClientId == id).ToListAsync();

            var viewModel = new ClientInsuranceViewModel
            {
                ClientData = policyholder,
                InsuranceData = insurances
            };

            return View(viewModel);
        }

        // GET: Policyholders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Policyholders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClientCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if(!result.Succeeded)
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }

            await _userManager.AddToRoleAsync(user, UserRoles.Client);

            var client = new Client
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                City = model.City,
                ZipCode = model.ZipCode,
                IdentityUserId = user.Id
            };

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Policyholders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var policyholder = await _context.Clients.FindAsync(id);
            if (policyholder == null)
            {
                return NotFound();
            }
            return View(policyholder);
        }

        // POST: Policyholders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Email,PhoneNumber,Address,City,ZipCode,IdentityUserId")] Client client)
        {
            if (id != client.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                foreach (var entry in ModelState)
                {
                    foreach (var modelError in ModelState)
                    {
                        Console.WriteLine($"Field: {modelError.Key}, Error: {string.Join(", ", modelError.Value.Errors.Select(e => e.ErrorMessage))}");
                    }
                }
                return View(client);
            }

            var existingClient = await _context.Clients.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);

            if (existingClient == null)
            {
                return NotFound();
            }

            bool emailChanged = !string.Equals(existingClient.Email, client.Email, StringComparison.OrdinalIgnoreCase);

            try
            {
                _context.Update(client);
                await _context.SaveChangesAsync();

                if (emailChanged && !string.IsNullOrEmpty(existingClient.IdentityUserId))
                {
                    var user = await _userManager.FindByIdAsync(existingClient.IdentityUserId);
                    if (user != null)
                    {
                        user.Email = client.Email;
                        user.NormalizedEmail = _userManager.NormalizeEmail(client.Email);
                        user.UserName = client.Email;
                        user.NormalizedUserName = _userManager.NormalizeEmail(client.Email);

                        var result = await _userManager.UpdateAsync(user);
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                            return RedirectToAction("Details", "Admin", new { id = client.Id });
                        }
                    }
                }

                return RedirectToAction("Details", "Admin", new { id = client.Id });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PolicyholderExists(client.Id))
                {
                    return NotFound();
                }
                throw;
            }
        }

        // GET: Policyholders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var policyholder = await _context.Clients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (policyholder == null)
            {
                return NotFound();
            }

            return View(policyholder);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = await _context.Clients.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);

            if (client == null)
            {
                return NotFound();
            }

            IdentityUser user = null;

            if(!string.IsNullOrEmpty(client.IdentityUserId))
            {
                user = await _userManager.FindByIdAsync(client.IdentityUserId);
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, "Failed to delete linked user.");
                    return View("Error");
                }
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PolicyholderExists(int id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }
    }
}
