using Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    [Table("Fund")]
    public class Fund : EntityBase
    {
        [Required(ErrorMessage = "Fund Name is required")]
        [StringLength(maximumLength: 24, ErrorMessage = "The Fund name can not exceed 24 characters.")]
        public string FundName { get; set; }

        [StringLength(maximumLength: 1024, ErrorMessage = "The Fund comments can not exceed 1024 characters.")]
        public string Comments { get; set; }
    }
}