namespace GarageBuddy.Web.ViewModels.User
{
    using System.ComponentModel.DataAnnotations;

    using static Common.Constants.EntityValidationConstants.ApplicationUser;

    public class LoginViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [StringLength(UserNameMaxLength, MinimumLength = UserNameMinLength)]
        public string Username { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [StringLength(PasswordMaxLength, MinimumLength = PasswordMinLength)]
        public string Password { get; set; } = null!;

        public bool RememberMe { get; set; } = false;

        public string? ReturnUrl { get; set; }
    }
}
