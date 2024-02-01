using System.ComponentModel.DataAnnotations;

namespace Data.DTOs
{
    public class ContributionForCreation : IValidatableObject
    {
        // this class contains validation rules for ContributionForCreation DTO

        [Required]
        public DateTime ContributionDate { get; set; }

        [Required]
        public Decimal Amount { get; set; }

        public int? CheckNumber { get; set; }

        [MaxLength(1024, ErrorMessage = "Comments cannot exceed 1024 characters.")]
        public string? Comments { get; set; }
        [Required]
        public int FundId { get; set; }
        [Required]
        public int ContributorId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // used to place multiple and combination validation rules will return a list of errors in response body.
            var results = new List<ValidationResult>();
            // database validation checks            
            using (APIDbContext _context = new APIDbContext())
            {
                if (!_context.Funds.Any(a => a.Id == FundId))
                {
                    results.Add(new ValidationResult("Fund Id was not found."));
                }
                if (!_context.Contributors.Any(a => a.Id == ContributorId))
                {
                    results.Add(new ValidationResult("Contributor Id was not found."));
                }
            }
            if (Amount == 0)
            {
                results.Add(new ValidationResult("Amount cannot be zero."));
            }
            if (Comments != null && Comments.Trim().Length > 1024)
            {
                results.Add(new ValidationResult("Comments cannot exceed 1024 characters."));
            }
            return results;
        }
    }
}