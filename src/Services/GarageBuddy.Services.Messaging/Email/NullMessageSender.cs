namespace GarageBuddy.Services.Messaging.Email
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class NullMessageSender : IEmailSender
    {
        public Task SendEmailAsync(
            string fromMail,
            string fromName,
            string toMail,
            string subject,
            string htmlContent,
            IEnumerable<EmailAttachment> attachments = null)
        {
            return Task.CompletedTask;
        }

        public Task SendEmailAsync(string toMail, string subject, string htmlContent)
        {
            return Task.CompletedTask;
        }
    }
}
