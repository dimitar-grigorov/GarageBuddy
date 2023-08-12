namespace GarageBuddy.Web.ViewModels.User
{
    using System.ComponentModel.DataAnnotations;

    using static Common.Constants.EntityValidationConstants.ApplicationUser;

    public class ResetPasswordFormModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        [StringLength(EmailMaxLength, MinimumLength = EmailMinLength)]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required]
        [MinLength(ResetPasswordTokenMinLength)]
        public string? Token { get; set; }
    }
}
