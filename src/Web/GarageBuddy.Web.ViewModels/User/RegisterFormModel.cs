namespace GarageBuddy.Web.ViewModels.User
{
    using System.ComponentModel.DataAnnotations;

    using static Common.Constants.EntityValidationConstants.ApplicationUser;
    using static Common.Constants.MessageConstants;

    public class RegisterFormModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        [StringLength(EmailMaxLength, MinimumLength = EmailMinLength)]
        public string Email { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = null!;

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare(nameof(Password), ErrorMessage = Errors.PasswordsDoNotMatch)]
        public string ConfirmPassword { get; set; } = null!;
    }
}
