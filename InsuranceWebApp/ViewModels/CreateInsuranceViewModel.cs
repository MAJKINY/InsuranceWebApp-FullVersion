using InsuranceWebApp.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace InsuranceWebApp.ViewModels
{
    public class CreateInsuranceViewModel
    {
        public Insurance InsuranceData { get; set; }
        [ValidateNever]
        public Client ClientData { get; set; }

        public CreateInsuranceViewModel()
        {
            InsuranceData = new Insurance();
            ClientData = new Client();
        }
    }
}
