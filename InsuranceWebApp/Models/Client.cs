using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsuranceWebApp.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? IdentityUserId { get; set; }
        [ValidateNever]
        public IdentityUser IdentityUser { get; set; } = null!;

        public List<Insurance>? Insurance { get; set; }
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
    }
}
