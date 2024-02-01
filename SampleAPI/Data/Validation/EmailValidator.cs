using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Data.Validation
{
    public class EmailValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) { return ValidationResult.Success; }

            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            if (!regex.IsMatch(value.ToString()))
            {
                return new ValidationResult($"Please enter a valid email address");
            }

            return ValidationResult.Success;
        }
    }
}
