namespace GarageBuddy.Services.Messaging.Email
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IEmailSender
    {
        Task SendEmailAsync(
            string fromMail,
            string fromName,
            string toMail,
            string subject,
            string htmlContent,
            IEnumerable<EmailAttachment> attachments = null);

        public Task SendEmailAsync(string toMail, string subject, string htmlContent);
    }
}
