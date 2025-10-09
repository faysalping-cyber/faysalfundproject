using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FaysalFundsInternal.Domain.Entities
{
    [Table("FAML_UNIT_HOLDER")]
    public class UnitHolder
    {
        [Key]
        public string UNIT_HOLDER_ID { get; set; }
        public string REGISTRATION_NO_2 { get; set; }
    }
}
