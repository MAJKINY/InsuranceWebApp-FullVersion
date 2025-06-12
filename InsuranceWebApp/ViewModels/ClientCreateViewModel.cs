using System.ComponentModel.DataAnnotations;

namespace InsuranceWebApp.ViewModels
{
    public class ClientCreateViewModel
    {
        [Required(ErrorMessage = "This value is mandatory")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = "";
        [Required(ErrorMessage = "This value is mandatory")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = "";
        [Required(ErrorMessage = "This value is mandatory")]
        [EmailAddress(ErrorMessage = "Please, enter valid email address")]
        public string Email { get; set; } = "";
        [Required(ErrorMessage = "This value is mandatory")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = "";
        [Required(ErrorMessage = "This value is mandatory")]
        public string Address { get; set; } = "";
        [Required(ErrorMessage = "This value is mandatory")]
        public string City { get; set; } = "";
        [Required(ErrorMessage = "This value is mandatory")]
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; } = "";

        [Required(ErrorMessage = "Enter password")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "{0} needs to have length of at least {2} characters and most {1} characters.", MinimumLength = 8)]
        [Display(Name = "Password")]
        public string Password { get; set; } = "";

        [Required(ErrorMessage = "Enter password")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare(nameof(Password), ErrorMessage = "Passwords need to match.")]
        public string ConfirmPassword { get; set; } = "";
    }
}
