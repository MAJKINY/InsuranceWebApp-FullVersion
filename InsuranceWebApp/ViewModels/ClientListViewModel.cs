using InsuranceWebApp.Models;

namespace InsuranceWebApp.ViewModels
{
    public class ClientListViewModel
    {
        public List<Client> Clients { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string SearchString {  get; set; }
    }
}
