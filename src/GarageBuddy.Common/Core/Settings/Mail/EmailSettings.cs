namespace GarageBuddy.Common.Core.Settings.Mail
{
    using System.ComponentModel.DataAnnotations;

    public class EmailSettings
    {
        [Required]
        public string SenderEmail { get; set; } = null!;

        [Required]
        public string SenderName { get; set; } = null!;

        public SendGridMailSettings SendGridSettings { get; set; } = new SendGridMailSettings();

        public SmtpMailSettings SmtpSettings { get; set; } = new SmtpMailSettings();
    }
}
