using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Domain.Interfaces
{
    public interface IEmailRepository
    {
 Task SendOtpEmailAsync(string toEmail, string otp);
        Task SendEmailAsync(string toEmail, string subject, string body, bool isHtml = false);
    }
}
