namespace GarageBuddy.Common.Constants
{
    public static class GlobalConstants
    {
        public const string SystemName = "GarageBuddy";

        public const string AdministratorRoleName = "Administrator";

        public const string ErrorRoute = "/Home/Errors";

        public const string MailTemplatePath = "MailTemplates";

        public const string ThemeErrorImagesPathTemplate = "/themes/mazer/dist/assets/compiled/svg/error-{0}.svg";

        /// <summary>
        /// Gets a default timeout (in milliseconds) before restarting the application.
        /// </summary>
        public static int RestartTimeout => 3000;
    }
}
