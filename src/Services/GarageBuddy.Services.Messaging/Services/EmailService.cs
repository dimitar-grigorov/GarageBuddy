namespace GarageBuddy.Services.Messaging.Services
{
    using System;
    using System.Threading.Tasks;

    using Common.Constants;
    using Common.Core.Wrapper;

    using Contracts;

    using GarageBuddy.Services.Messaging.Email;

    using Microsoft.Extensions.Logging;

    using static GarageBuddy.Common.Constants.MessageConstants;

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
                return await Result.SuccessAsync(string.Format(MessageConstants.Success.PasswordResetMailSent, userEmail));
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, Errors.GeneralErrorSendEmail);
                return await Result.FailAsync(Errors.GeneralErrorSendEmail);
            }
        }
    }
}
