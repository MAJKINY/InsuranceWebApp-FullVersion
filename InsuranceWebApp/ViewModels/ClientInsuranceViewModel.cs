using InsuranceWebApp.Models;

namespace InsuranceWebApp.ViewModels
{
    public class ClientInsuranceViewModel
    {
        public Client ClientData { get; set; }
        public List<Insurance> InsuranceData { get; set; }
    }
}
