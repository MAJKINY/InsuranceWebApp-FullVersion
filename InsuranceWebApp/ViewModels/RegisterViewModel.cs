using System.ComponentModel.DataAnnotations;

namespace InsuranceWebApp.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Enter email address")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Enter password")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "{0} needs to have length of at least {2} characters and most {1} characters.", MinimumLength = 8)]
        [Display(Name = "Password")]
        public string Password { get; set; } = "";

        [Required(ErrorMessage = "Enter password")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [Compare(nameof(Password), ErrorMessage = "Passwords need to match.")]
        public string ConfirmPassword { get; set; } = "";
    }
}
