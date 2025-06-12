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
    [Authorize(Roles = UserRoles.Client)]
    public class ClientController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IClientService _clientService;
        private readonly UserManager<IdentityUser> _userManager;
        SignInManager<IdentityUser> _signInManager;

        public ClientController(ApplicationDbContext context, IClientService clientService, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _clientService = clientService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: Policyholders/Details/5
        public async Task<IActionResult> Details()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var client = await _context.Clients
                .Include(c => c.Insurance)
                .FirstOrDefaultAsync(c => c.IdentityUserId == userId);

            if (client == null)
            {
                return NotFound();
            }

            var viewModel = new ClientInsuranceViewModel
            {
                ClientData = client,
                InsuranceData = client.Insurance
            };

            return View(viewModel);
        }

        // GET: Policyholders/Edit/5
        public async Task<IActionResult> Edit()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var client = await _context.Clients.FirstOrDefaultAsync(c => c.IdentityUserId == userId);

            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Policyholders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,FirstName,LastName,Email,PhoneNumber,Address,City,ZipCode,IdentityUserId")] Client client)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var existingClient = await _context.Clients
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.IdentityUserId == currentUserId);

            if (existingClient == null || existingClient.Id != client.Id)
            {
                return NotFound();
            }

            bool emailChanged = !string.Equals(existingClient.Email, client.Email, StringComparison.OrdinalIgnoreCase);

            if (!ModelState.IsValid)
            {
                return View(client);
            }

            try
            {
                _context.Update(client);
                await _context.SaveChangesAsync();

                if (emailChanged && !string.IsNullOrEmpty(client.IdentityUserId))
                {
                    var user = await _userManager.FindByIdAsync(client.IdentityUserId);

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
                            return View(client);
                        }

                        await _signInManager.SignOutAsync();
                        return RedirectToAction("Login", "Account");
                    }
                }

                return RedirectToAction(nameof(Details));
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

        private bool PolicyholderExists(int id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }
    }
}
