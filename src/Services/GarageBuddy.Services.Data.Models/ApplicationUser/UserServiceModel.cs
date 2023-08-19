namespace GarageBuddy.Services.Data.Models.ApplicationUser
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Common.Attributes;

    using GarageBuddy.Data.Models;

    using Mapping;

    using static GarageBuddy.Common.Constants.EntityValidationConstants.ApplicationUser;

    public class UserServiceModel : IMapFrom<ApplicationUser>
    {
        public Guid? Id { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(EmailMaxLength, MinimumLength = EmailMinLength)]
        [DataType(DataType.EmailAddress)]
        [Sanitize]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(UserNameMaxLength, MinimumLength = UserNameMinLength)]
        [Sanitize]
        public string UserName { get; set; } = null!;

        [DataType(DataType.Password)]
        [Sanitize]
        public string? Password { get; set; }

        public IEnumerable<string> Roles { get; set; } = new List<string>();
    }
}
