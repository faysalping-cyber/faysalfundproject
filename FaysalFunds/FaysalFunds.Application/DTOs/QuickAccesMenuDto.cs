using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Application.DTOs
{
    public class QuickAccesMenuDto
    {
    
        public long ID { get; set; }
        public string NAME { get; set; }

        public byte[]? ICON { get; set; } // Base64 icon
        //public long? TRANSACTIONFEATUREID { get; set; }
        public int ACTIVE { get; set; } // 0 or 1

    }

  
    public class UserQuickAccessResponseDto
    {
        public List<QuickAccesMenuDto> ActiveItems { get; set; }
        public List<QuickAccesMenuDto> AvailableItems { get; set; }
    }

}
