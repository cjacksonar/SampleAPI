using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    [Table("State")]
    public class State
    {

        [Key]
        [DataType(DataType.Text), MaxLength(2)]
        public string StateCode { get; set; }
        [DataType(DataType.Text), MaxLength(40)]
        public string StateName { get; set; }
    }
}