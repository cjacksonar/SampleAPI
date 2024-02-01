using Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    [Table("Contributor")]
    public class Contributor : EntityBase
    {
        [Required]
        [DataType(DataType.Text), MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Text), MaxLength(50)]
        public string Address { get; set; }

        [Required]
        [DataType(DataType.Text), MaxLength(50)]
        public string City { get; set; }

        [Required]
        [DataType(DataType.Text), MaxLength(2)]
        public string StateCode { get; set; }

        [Required]
        [DataType(DataType.Text), MaxLength(10)]
        public string ZipCode { get; set; }

        [DataType(DataType.Text), MaxLength(14)]
        public string? Phone { get; set; }

        [DataType(DataType.Text), MaxLength(50)]
        public string? Email { get; set; }

        [DataType(DataType.Text), MaxLength(1024)]
        public string? Comments { get; set; }
    }
}