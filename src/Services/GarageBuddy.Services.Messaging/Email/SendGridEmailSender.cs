namespace GarageBuddy.Services.Messaging.Email
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Common.Core.Settings.Mail;

    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    using SendGrid;
    using SendGrid.Helpers.Mail;

    using static GarageBuddy.Common.Constants.ErrorMessageConstants;

    public class SendGridEmailSender : IEmailSender
    {
        private readonly EmailSettings options;
        private readonly ILogger<SendGridEmailSender> logger;

        public SendGridEmailSender(IOptions<EmailSettings> options,
            ILogger<SendGridEmailSender> logger)
        {
            this.options = options.Value;
            this.logger = logger;
        }

        public async Task SendEmailAsync(string fromMail, string fromName, string toMail,
            string subject, string htmlContent, IEnumerable<EmailAttachment> attachments = null)
        {
            if (string.IsNullOrWhiteSpace(subject) && string.IsNullOrWhiteSpace(htmlContent))
            {
                throw new ArgumentException(ErrorMailSubjectAndMessageNotProvided);
            }

            var fromAddress = new EmailAddress(fromMail, fromName);
            var toAddress = new EmailAddress(toMail);
            var message = MailHelper.CreateSingleEmail(fromAddress, toAddress, subject, null, htmlContent);

            var attachmentsList = attachments?.ToList();
            if (attachmentsList?.Any() == true)
            {
                foreach (var attachment in attachmentsList)
                {
                    message.AddAttachment(attachment.FileName, Convert.ToBase64String(attachment.Content), attachment.MimeType);
                }
            }

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            message.SetClickTracking(false, false);

            try
            {
                var apiKey = options.SendGridSettings.ApiKey;
                if (string.IsNullOrWhiteSpace(apiKey))
                {
                    throw new ArgumentException(ErrorSendGridApiKeyNotProvided);
                }

                var client = new SendGridClient(apiKey);
                var response = await client.SendEmailAsync(message);

                if (response != null)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        logger.LogInformation("Email sent to {toMail}. Status code: {StatusCode}.", toMail, response.StatusCode);
                    }
                    else
                    {
                        logger.LogError("Email not sent to {toMail}. Status code: {StatusCode}.", toMail, response.StatusCode);
                    }

                    var responseBody = await response.Body.ReadAsStringAsync();
                    logger.LogDebug("Response body: {responseBody}", responseBody);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Message: {Message}", ex.Message);
                throw;
            }
        }

        public async Task SendEmailAsync(string toMail, string subject, string htmlContent)
        {
            var fromMail = options.SenderEmail;
            var fromName = options.SenderName;
            await SendEmailAsync(fromMail, fromName, toMail, subject, htmlContent);
        }
    }
}
