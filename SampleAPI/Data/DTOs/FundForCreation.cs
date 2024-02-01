using System.ComponentModel.DataAnnotations;

namespace Data.DTOs
{
    public class FundForCreation : IValidatableObject
    {
        // this class contains validation rules for FundForCreation DTO        

        [Required]
        [MaxLength(24, ErrorMessage = "The Fund description can not have more than 24 characters.")]
        public string FundName { get; set; }

        [MaxLength(1024, ErrorMessage = "Comments cannot exceed 1024 characters.")]
        public string Comments { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // used to put multiple and combination validation rules will return a list of errors in response body.
            var results = new List<ValidationResult>();
            if (FundName.Trim().Length > 24)
            {
                results.Add(new ValidationResult("The Fund description can not have more than 24 characters."));
            }
            if (Comments != null)
            {
                if (Comments.Trim().Length > 1024)
                {
                    results.Add(new ValidationResult("Comments cannot exceed 1024 characters."));
                }
            }
            return results;
        }
    }
}