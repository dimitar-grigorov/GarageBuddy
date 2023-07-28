namespace GarageBuddy.Services.Data.Models.ApplicationUser
{
    using System.ComponentModel.DataAnnotations;

    // TODO: Check if validations are needed
    public class RegisterUserServiceModel
    {
        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public string UserName { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}
