using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    [Table("Registration")]
    public class Registration
    {
        [Required]
        [Key]
        [DataType(DataType.Text), MaxLength(15)]
        public string ProductId { get; set; }

        [Required]
        [DataType(DataType.Text), MaxLength(100)]
        public string OrganizationName { get; set; }

        [Required]
        [DataType(DataType.Text), MaxLength(50)]
        public string PrimaryContactName { get; set; }

        [Required]
        [DataType(DataType.Text), MaxLength(255)]
        public string PrimaryContactEmail { get; set; }

        [Required]
        [DataType(DataType.Text), MaxLength(24)]
        public string AdminUserId { get; set; }

        [Required]
        [DataType(DataType.Text), MaxLength(24)]
        public string AdminUserPassword { get; set; }

    }
}