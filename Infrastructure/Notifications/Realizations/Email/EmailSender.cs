using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.EmailNotifications
{
    public class SenderOptions
    {
        public const string EmailBlock = "Email";
        public string SourceEmail { get; set; }
        public string SourcePassword { get; set; }
        public int Port { get; set; }
        public int UseSSL { get; set; }
    }

    public class EmailSender
    {
        private static SenderOptions _secrets { get; set; }

        public EmailSender(IConfiguration config)
        {
            _secrets = new SenderOptions();
            config.GetSection(SenderOptions.EmailBlock).Bind(_secrets);
        }

        public async Task<bool> SendEmailAsync(string email, string subject, string message) =>
            await TrySend(BuildMessage(email, subject, message));

        private static async Task<bool> TrySend(MimeMessage emailMessage)
        {
            try
            {
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("smtp.mail.ru", _secrets.Port, _secrets.UseSSL == 1);
                    await client.AuthenticateAsync(_secrets.SourceEmail, _secrets.SourcePassword);
                    await client.SendAsync(emailMessage);

                    await client.DisconnectAsync(true);
                }
            }
            catch 
            { 
                return false;
            }

            return true;
        }

        private MimeMessage BuildMessage(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Admimistration s-bet", _secrets.SourceEmail));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;

            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            return emailMessage;
        }
    }
}
