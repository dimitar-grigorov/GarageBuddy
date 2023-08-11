namespace GarageBuddy.Services.Messaging.Services
{
    using System;
    using System.Threading.Tasks;

    using Common.Core.Wrapper;

    using Contracts;

    using GarageBuddy.Services.Messaging.Email;

    using Microsoft.Extensions.Logging;

    using static GarageBuddy.Common.Constants.ErrorMessageConstants;
    using static GarageBuddy.Common.Constants.SuccessMessageConstants;

    public class EmailService : IEmailService
    {
        private readonly IEmailSender emailSender;
        private readonly ILogger<EmailService> logger;

        public EmailService(
            IEmailSender emailSender,
            ILogger<EmailService> logger)
        {
            this.emailSender = emailSender;
            this.logger = logger;
        }

        public async Task<IResult> SendResetPasswordEmail(string userEmail, string htmlContent)
        {
            try
            {
                await this.emailSender.SendEmailAsync(userEmail, "Reset Password", htmlContent);
                return await Result.SuccessAsync(string.Format(SuccessPasswordResetMailSent, userEmail));
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ErrorGeneralSendEmail);
                return await Result.FailAsync(ErrorGeneralSendEmail);
            }
        }
    }
}
