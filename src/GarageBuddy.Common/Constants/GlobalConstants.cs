namespace GarageBuddy.Common.Constants
{
    public static class GlobalConstants
    {
        public const string SystemName = "GarageBuddy";

        public const string AdministratorRoleName = "Administrator";

        public const string AdminArea = "Admin";

        public const string ErrorRoute = "/Home/Error";

        public const string MailTemplatePath = "MailTemplates";

        public const string ThemeErrorImagesPathTemplate = "/themes/mazer/dist/assets/compiled/svg/error-{0}.svg";

        public const string IncludeDeletedFilterName = "includeDeleted";

        public const string DefaultDateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// Gets a default timeout (in milliseconds) before restarting the application.
        /// </summary>
        public static int RestartTimeout => 3000;
    }
}
