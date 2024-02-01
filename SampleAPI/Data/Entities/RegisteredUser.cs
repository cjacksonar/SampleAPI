using Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    [Table("RegisteredUser")]
    public class RegisteredUser : EntityBase
    {
        [Required]
        [DataType(DataType.Text), MaxLength(24)]
        public string UserId { get; set; }

        [Required]
        [DataType(DataType.Text), MaxLength(12)]
        public string UserPassword { get; set; }

        [Required]
        [DataType(DataType.Text), MaxLength(50)]
        public string UserName { get; set; }

        [DataType(DataType.Text), MaxLength(255)]
        public string UserEmail { get; set; }

        public int UserRoleId { get; set; }

        public UserRole UserRole { get; set; }

        public Boolean AllowEditing { get; set; } = true;

        public int NumberOfLogins { get; set; }
        [Required]
        public DateTime LastLogin { get; set; }

    }
}