using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

namespace HACT.Services
{
    public class EmailSender
    {
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPass;
        private readonly string _fromEmail;

        public EmailSender(string smtpHost, int smtpPort, string smtpUser, string smtpPass, string fromEmail)
        {
            _smtpHost = smtpHost;
            _smtpPort = smtpPort;
            _smtpUser = smtpUser;
            _smtpPass = smtpPass;
            _fromEmail = fromEmail;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body, string fromEmail)
        {
            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(fromEmail));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = body };

            using var client = new SmtpClient();
            await client.ConnectAsync(_smtpHost, _smtpPort, MailKit.Security.SecureSocketOptions.None);
            await client.AuthenticateAsync(_smtpUser, _smtpPass);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

    }
}
