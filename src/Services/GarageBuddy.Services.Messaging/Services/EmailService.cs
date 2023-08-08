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

        public async Task SendResetPasswordEmail(string userEmail, string resetPasswordUrl)
        {
            var token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(resetPasswordUrl));
            var htmlContent = GenerateHtmlFromTemplate(resetPasswordUrl, "ResetPasswordEmailTemplate.html");

            await this.emailSender.SendEmailAsync(userEmail, "Reset Password", htmlContent);
        }


        private string GenerateHtmlFromTemplate(string resetPasswordUrl, string template)
        {
            var executingAssemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var templatePath = Path.Combine(executingAssemblyPath!, "/Email/Templates/");
            var templateFullPath = Path.Combine(templatePath, template);

            if (!File.Exists(templateFullPath))
            {
                throw new FileNotFoundException($"Template {template} not found at {templateFullPath}");
            }

            var templateContent = File.ReadAllText(templatePath + template);

            return string.Empty;
        }
    }
}
