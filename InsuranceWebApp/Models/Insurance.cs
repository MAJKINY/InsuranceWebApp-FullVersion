using System.ComponentModel.DataAnnotations;

namespace InsuranceWebApp.Models
{
    public class Insurance : IValidatableObject
    {
        public int Id { get; set; }
        [Required]
        public int ClientId { get; set; }
        public Client? Client { get; set; }
        [Required]
        [Display(Name = "Insurance type")]
        public TypeOfInsurance InsuranceType { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public long Amount { get; set; }
        [Required(ErrorMessage = "This field is required. For example: House.")]
        [Display(Name = "Subject of insurance")]
        public string SubjectOfInsurance { get; set; } = "";
        [DataType(DataType.Date)]
        [Display(Name = "Start of insurance")]
        public DateTime StartOfInsurance { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "End of insurance")]
        public DateTime EndOfInsurance { get; set; }

        public enum TypeOfInsurance 
        { 
            [Display(Name = "Health Insurance")] HealthInsurance,
            [Display(Name = "Property Insurance")] PropertyInsurance 
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(EndOfInsurance < StartOfInsurance)
            {
                yield return new ValidationResult(
                    "End date must be the same day or after the start date.",
                    new[] { nameof(EndOfInsurance) });
            }
        }
    }
}
