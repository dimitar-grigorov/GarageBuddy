namespace GarageBuddy.Common.Constants
{
    public static class GlobalValidationConstants
    {
        public const int DefaultDecimalPrecision = 18;
        public const int DefaultDecimalScale = 2;

        // public const int DefaultDecimalMinValue = 0;
        // public const int DefaultDecimalMaxValue = 1_000_000_000;
        public const int DefaultDescriptionMaxLength = 2000;

        public const int DefaultAddressMaxLength = 200;

        public const int DefaultEmailMaxLength = 150;

        public const int UrlMaxLength = 2048;

        public const string CoordinatesRegex =
            @"^[-+]?([1-8]?\d(\.\d+)?|90(\.0+)?),\s*[-+]?(180(\.0+)?|((1[0-7]\d)|([1-9]?\d))(\.\d+)?)$";
    }
}
