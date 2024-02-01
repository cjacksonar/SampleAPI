using Data.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Data.DTOs
{
    public class RegisteredUserForCreation : IValidatableObject
    {

        [MaxLength(24, ErrorMessage = "User Id cannot exceed 24 characters.")]
        public string UserId { get; set; }

        [MaxLength(24, ErrorMessage = "User password cannot exceed 24 characters.")]
        public string UserPassword { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "User Name can not have more than 50 characters.")]
        public string UserName { get; set; }

        [EmailValidator]
        public string? UserEmail { get; set; }

        [Required]
        public int UserRoleId { get; set; }
        [DefaultValue(2)]

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // used to put multiple and combination validation rules will return a list of errors in response body.
            var results = new List<ValidationResult>();
            if (UserId.Trim().Length > 24)
            {
                results.Add(new ValidationResult("User Id Name can not have more than 24 characters."));
            }
            if (UserPassword.Trim().Length > 24)
            {
                results.Add(new ValidationResult("User Password can not have more than 24 characters."));
            }

            return results;
        }
    }
}