namespace GarageBuddy.Services.Messaging.Email
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    using GarageBuddy.Common.Core.Settings.Mail;

    using MailKit.Net.Smtp;
    using MailKit.Security;

    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    using MimeKit;

    public class SmtpMailSender : IEmailSender
    {
        private readonly EmailSettings options;
        private readonly ILogger<SmtpMailSender> logger;

        public SmtpMailSender(IOptions<EmailSettings> options,
            ILogger<SmtpMailSender> logger)
        {
            this.options = options.Value;
            this.logger = logger;
        }

        public async Task SendEmailAsync(
            string fromMail,
            string fromName,
            string toMail,
            string subject,
            string htmlContent,
            IEnumerable<EmailAttachment> attachments = null)
        {
            try
            {
                logger.LogInformation(htmlContent);

                fromMail ??= options.SenderEmail;
                var email = new MimeMessage
                {
                    From = { MailboxAddress.Parse(fromMail) },
                    Sender = new MailboxAddress(fromName, fromMail),
                    Subject = subject,
                    To = { MailboxAddress.Parse(toMail) },
                };

                email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = htmlContent,
                };

                var multipart = new Multipart("mixed");
                email.Body = multipart;

                // Add the HTML body part
                multipart.Add(new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = htmlContent,
                });

                // Add attachments
                if (attachments != null)
                {
                    foreach (var attachment in attachments)
                    {
                        var attachmentPart = new MimePart(attachment.MimeType)
                        {
                            Content = new MimeContent(new MemoryStream(attachment.Content)),
                            ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                            FileName = attachment.FileName,
                        };

                        multipart.Add(attachmentPart);
                    }
                }

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(
                    options.SmtpSettings.Host,
                    options.SmtpSettings.Port,
                    SecureSocketOptions.Auto);
                await smtp.AuthenticateAsync(
                    options.SmtpSettings.UserName,
                    options.SmtpSettings.Password);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
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
