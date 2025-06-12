using InsuranceWebApp.Data;
using InsuranceWebApp.Interfaces;
using InsuranceWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace InsuranceWebApp.Services
{
    public class ClientService : IClientService
    {
        private readonly ApplicationDbContext _context;

        public ClientService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Client> GetClientByUserIdAsync(string userId)
        {
            return await _context.Clients
                .Include(c => c.Insurance)
                .FirstOrDefaultAsync(c => c.IdentityUserId == userId);
        }
    }
}
