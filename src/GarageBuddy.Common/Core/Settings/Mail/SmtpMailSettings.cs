namespace GarageBuddy.Common.Core.Settings.Mail
{
    using System.ComponentModel.DataAnnotations;

    public class SmtpMailSettings
    {
        [Required]
        public string Host { get; set; } = string.Empty;

        [Range(1, 65535)]
        public int Port { get; set; } = 587;

        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
