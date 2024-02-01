using Data.Validation;
using System.ComponentModel;    // added so could use DefaultValue data annotation
using System.ComponentModel.DataAnnotations;

namespace Data.DTOs
{
    public class ContributorForCreation : IValidatableObject
    {
        // this class contains validation rules for ContributorForCreation DTO

        [Required]
        [MaxLength(50, ErrorMessage = "The Contributor name can not have more than 50 characters.")]
        public string Name { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "Address can not be more than 50 characters.")]
        public string Address { get; set; }

        [Required]
        [MaxLength(30, ErrorMessage = "City can not be more than 30 characters.")]

        public string City { get; set; }
        [Required]
        [MaxLength(2, ErrorMessage = "State code can not be more than 2 characters.")]
        [DefaultValue("AR")]
        public string StateCode { get; set; }
        [Required]
        [MaxLength(10, ErrorMessage = "Zip can not be more than 10 characters.")]
        [DefaultValue("72908")]
        public string ZipCode { get; set; }


        [MaxLength(14, ErrorMessage = "Phone cannot exceed 14 characters.")]
        public string? Phone { get; set; }

        [EmailValidator]
        public string? Email { get; set; }

        [MaxLength(1024, ErrorMessage = "Comments cannot exceed 1024 characters.")]
        public string? Comments { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // used to put multiple and combination validation rules will return a list of errors in response body.
            var results = new List<ValidationResult>();
            if (Name.Trim().Length > 50)
            {
                results.Add(new ValidationResult("The Contributor name can not have more than 50 characters."));
            }
            // database validation checks            
            using (APIDbContext _context = new APIDbContext())
            {
                if (!_context.States.Any(a => a.StateCode == StateCode.Trim()))
                {
                    results.Add(new ValidationResult("State Code is invalid."));
                }
            }
            if (Comments != null && Comments.Trim().Length > 1024)
            {
                results.Add(new ValidationResult("Comments cannot exceed 1024 characters."));
            }
            return results;
        }
    }
}