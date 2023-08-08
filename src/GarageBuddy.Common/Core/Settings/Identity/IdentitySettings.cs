namespace GarageBuddy.Common.Core.Settings.Identity
{
    public class IdentitySettings
    {
        public class PasswordSettings
        {
            public bool RequireDigit { get; set; }

            public bool RequireLowercase { get; set; }

            public bool RequireUppercase { get; set; }

            public bool RequireNonAlphanumeric { get; set; }

            public int RequiredLength { get; set; }
        }

        public class SignInSettings
        {
            public bool RequireConfirmedEmail { get; set; }
        }
    }
}
