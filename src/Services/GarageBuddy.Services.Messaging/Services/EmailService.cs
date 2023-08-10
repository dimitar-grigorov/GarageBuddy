namespace GarageBuddy.Services.Messaging.Services
{
    using System.IO;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    using Contracts;

    using GarageBuddy.Services.Messaging.Email;

    using Microsoft.AspNetCore.WebUtilities;

    public class EmailService : IEmailService
    {
        private readonly IEmailSender emailSender;

        public EmailService(IEmailSender emailSender)
        {
            this.emailSender = emailSender;
        }

        public async Task SendResetPasswordEmail(string userEmail, string htmlContent)
        {
            await this.emailSender.SendEmailAsync(userEmail, "Reset Password", htmlContent);
        }
    }
}
