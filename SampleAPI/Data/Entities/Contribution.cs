using Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    [Table("Contribution")]
    public class Contribution : EntityBase
    {
        [Required]
        public DateTime ContributionDate { get; set; }
        [Required]
        public Decimal Amount { get; set; }
        public int? CheckNumber { get; set; }
        [DataType(DataType.Text), MaxLength(1024)]
        public string? Comments { get; set; }
        public int FundId { get; set; }        
        public int ContributorId { get; set; }
       
    }
}