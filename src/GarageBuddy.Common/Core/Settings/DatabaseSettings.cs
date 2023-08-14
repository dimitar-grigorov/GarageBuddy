namespace GarageBuddy.Common.Core.Settings
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class DatabaseSettings : IValidatableObject
    {
        public string DbProvider { get; set; } = string.Empty;

        public string DefaultConnection { get; set; } = string.Empty;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(DbProvider))
            {
                yield return new ValidationResult(
                    $"{nameof(DatabaseSettings)}.{nameof(DbProvider)} is not configured",
                    new[] { nameof(DbProvider) });
            }

            if (string.IsNullOrEmpty(DefaultConnection))
            {
                yield return new ValidationResult(
                    $"{nameof(DatabaseSettings)}.{nameof(DefaultConnection)} is not configured",
                    new[] { nameof(DefaultConnection) });
            }
        }
    }
}
