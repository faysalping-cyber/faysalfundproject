using FaysalFunds.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Application.Services.IServices
{
    public interface IEmailService
    {
        Task SendOtpEmailAsync(string toEmail, string otp);
        Task SendEmailAsync(string toEmail, AccountOpening onboardingData = null);

    }
}
