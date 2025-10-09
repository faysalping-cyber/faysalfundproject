using FaysalFunds.Common;
using FaysalFunds.Common.APIException;
using FaysalFunds.Domain.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace FaysalFunds.Application.Repositories
{
    public class EmailRepository : IEmailRepository
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<EmailRepository> _logger;

        public EmailRepository(IOptions<EmailSettings> emailSettings, ILogger<EmailRepository> logger)
        {
            _emailSettings = emailSettings.Value ?? throw new ApiException(nameof(emailSettings));
            _logger = logger ?? throw new ApiException(nameof(logger));
        }

        public async Task SendOtpEmailAsync(string toEmail, string otp)
        {
            if (string.IsNullOrWhiteSpace(toEmail))
                throw new ApiException("Recipient email cannot be empty.", nameof(toEmail));
            if (string.IsNullOrWhiteSpace(otp))
                throw new ApiException("OTP cannot be empty.", nameof(otp));

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = "One-Time Password (OTP) – Faysal Funds";

            var bodyText = $@"
Dear Valued Customer,

Your OTP is {otp} and is valid for 5 minutes.

Do not share this with anyone. If you have not requested an OTP, kindly call Faysal Funds Islami Customer Care at 021-111 329 725.

Regards,
Faysal Funds
";

            message.Body = new TextPart("plain")
            {
                Text = bodyText.Trim()
            };

            await SendMessageAsync(message);
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body, bool isHtml = false)
        {
            if (string.IsNullOrWhiteSpace(toEmail))
                throw new ApiException("Recipient email cannot be empty.", nameof(toEmail));
            if (string.IsNullOrWhiteSpace(subject))
                throw new ApiException("Subject cannot be empty.", nameof(subject));
            if (string.IsNullOrWhiteSpace(body))
                throw new ApiException("Body cannot be empty.", nameof(body));

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = subject;

            message.Body = isHtml
                ? new TextPart("html") { Text = body }
                : new TextPart("plain") { Text = body };

            await SendMessageAsync(message);
        }

        private async Task SendMessageAsync(MimeMessage message)
        {
            try
            {
                _logger.LogInformation("Attempting to connect to SMTP server: {SmtpServer}:{SmtpPort}", _emailSettings.SmtpServer, _emailSettings.SmtpPort);
                using var smtp = new MailKit.Net.Smtp.SmtpClient();
                smtp.Timeout = 60000; // Set timeout to 60 seconds
                await smtp.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
                _logger.LogInformation("Connected to SMTP server. Authenticating with {SenderEmail}", _emailSettings.SenderEmail);
                await smtp.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.Password);
                _logger.LogInformation("Authenticated. Sending email...");
                await smtp.SendAsync(message);
                _logger.LogInformation("Email sent successfully.");
                await smtp.DisconnectAsync(true);
                _logger.LogInformation("Disconnected from SMTP server.");
            }
            catch (SmtpCommandException ex)
            {
                _logger.LogError(ex, "SMTP command error: {ErrorMessage}, StatusCode: {StatusCode}", ex.Message, ex.StatusCode);
                throw;
            }
            catch (SmtpProtocolException ex)
            {
                _logger.LogError(ex, "SMTP protocol error: {ErrorMessage}", ex.Message);
                throw;
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                _logger.LogError(ex, "Socket error: {ErrorMessage}, ErrorCode: {ErrorCode}", ex.Message, ex.SocketErrorCode);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email: {ErrorMessage}", ex.Message);
                throw;
            }
        }
    }
}