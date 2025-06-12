using InsuranceWebApp.Models;

namespace InsuranceWebApp.Interfaces
{
    public interface IClientService
    {
        Task<Client> GetClientByUserIdAsync(string userId);
    }
}
