namespace GarageBuddy.Web.ViewModels.User
{
    using System.ComponentModel.DataAnnotations;

    using static Common.Constants.EntityValidationConstants.ApplicationUser;

    public class ForgotPasswordFormModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Recovery E-mail")]
        [StringLength(EmailMaxLength, MinimumLength = EmailMinLength)]
        public string Email { get; set; } = null!;
    }
}
