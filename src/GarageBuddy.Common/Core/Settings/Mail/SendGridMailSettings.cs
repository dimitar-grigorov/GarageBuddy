namespace GarageBuddy.Common.Core.Settings.Mail
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class SendGridMailSettings : IValidatableObject
    {
        public string ApiKey { get; set; } = string.Empty;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(ApiKey))
            {
                yield return new ValidationResult(
                    $"{nameof(SendGridMailSettings)}.{nameof(ApiKey)} is not configured",
                    new[] { nameof(ApiKey) });
            }

            if (ApiKey.Length < 10)
            {
                yield return new ValidationResult(
                    $"{nameof(SendGridMailSettings)}.{nameof(ApiKey)} is to short",
                    new[] { nameof(ApiKey) });
            }
        }
    }
}
