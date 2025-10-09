using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Domain.Entities
{
    [Table("JWTBLACKLIST")]
    public class JwtBlacklist
    {
        public int ID { get; set; }
        public string TOKEN { get; set; }
        public DateTime EXPIRATIONDATE { get; set; }
    }
}
