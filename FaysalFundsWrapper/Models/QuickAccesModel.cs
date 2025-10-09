using System.ComponentModel.DataAnnotations.Schema;

namespace FaysalFundsWrapper.Models
{
    public class QuickAccesModel
    {

        public long ID { get; set; }
        public string NAME { get; set; }

        public byte[]? ICON { get; set; } 

        public int ACTIVE { get; set; } 

    }
    public class UserQuickAccessDto
    {
        public long USERID { get; set; }
        public long QUICKACCESSID { get; set; }
    }
    public class UserQuickAccessResponseDto
    {
        public List<QuickAccesModel> ActiveItems { get; set; }
        public List<QuickAccesModel> AvailableItems { get; set; }
    }

}
