using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Application.DTOs
{
    public class ResetPasswordModel
    {
        public string Email { get; set; } = string.Empty;
        //public string CellNo { get; set; } = string.Empty;
        //public string CNIC { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
    public class ChangePasswordModel
    {
        public long UserId { get; set; }
        public string OldPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}