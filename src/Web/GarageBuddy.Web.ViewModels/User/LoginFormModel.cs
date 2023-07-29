namespace GarageBuddy.Web.ViewModels.User
{
    using System.ComponentModel.DataAnnotations;

    using static Common.Constants.EntityValidationConstants.ApplicationUser;

    public class LoginFormModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        [StringLength(EmailMaxLength, MinimumLength = EmailMinLength)]
        public string Email { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        public bool RememberMe { get; set; } = false;

        // TODO: Implement returnUrl
        public string? ReturnUrl { get; set; }
    }
}
