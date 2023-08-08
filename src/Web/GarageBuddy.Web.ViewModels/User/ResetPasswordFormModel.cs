namespace GarageBuddy.Web.ViewModels.User
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

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
        [StringLength(ResetPasswordTokenMaxLength, MinimumLength = ResetPasswordTokenMinLength)]
        public string? Token { get; set; }
    }
}
