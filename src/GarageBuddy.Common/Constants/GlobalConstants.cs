namespace GarageBuddy.Common.Constants
{
    public static class GlobalConstants
    {
        public const string SystemName = "GarageBuddy";

        public const string AllRolesKey = "AllRoles";

        public const string AdminArea = "Admin";

        public const string ErrorRoute = "/Home/Error";

        public const string UserLoginRoute = "/User/Login";

        public const string UserLogoutRoute = "/User/Logout";

        public const string MailTemplatePath = "MailTemplates";

        public const string ThemeErrorImagesPathTemplate = "/themes/mazer/dist/assets/compiled/svg/error-{0}.svg";

        public const string IncludeDeletedFilterName = "includeDeleted";

        public const string IdFilterName = "id";

        public const string DefaultDateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        public const string DefaultDateFormat = "yyyy-MM-dd";

        public const int MinValidYear = 1900;

        public const string NoValue = "None";

        public static class Roles
        {
            public const string Administrator = "Administrator";

            public const string Manager = "Manager";

            public const string Mechanic = "Mechanic";

            public const string User = "User";
        }

        public static class Policies
        {
            public const string AdminPolicy = "AdminPolicy";

            public const string ManagerPolicy = "ManagerPolicy";

            public const string MechanicPolicy = "MechanicPolicy";
        }
    }
}
