namespace GarageBuddy.Web.ViewModels.MailTemplates
{
    using System.ComponentModel.DataAnnotations;

    public class ForgotPasswordMailViewModel
    {
        [Required]
        [Url]
        public string ResetPasswordUrl { get; set; } = null!;

        [Required]
        public string ApplicationName { get; set; } = null!;
    }
}
