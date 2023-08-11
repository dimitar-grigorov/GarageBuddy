namespace GarageBuddy.Common.Constants
{
    public static class MessageConstants
    {
        public static class Errors
        {
            public const string GeneralError = "Something went wrong!";

            public const string NoEntityWithPropertyFound = "Entity {0} with property {1} was not found!";

            public const string DeserializationFailed = "Deserialization of the file {0} failed!";

            public const string InvalidDirectoryPath = "Invalid directory path!";

            public const string CannotBeNullOrWhitespace = "{0} cannot be null or whitespace.";

            public const string InvalidUsernameOrPassword = "Invalid username or password.";

            public const string AccountLockedOut = "Account locked out. Try again later";

            public const string UserNotFound = "User not found.";

            public const string PasswordsDoNotMatch = "Passwords do not match.";

            public const string SomethingWentWrong = "Something went wrong. Please try again.";

            public const string ConnectionStringWrongFormat = "Connection string is in wrong format!";

            public const string DatabaseCreationFailed = "Database creation failed! Errors: {0}";

            public const string DatabaseNotExists = "Database does not exist!";

            public const string DatabaseInstallationFailed = "Database installation failed! Errors: {0}";

            public const string SendGridApiKeyNotProvided = "SendGrid API key is not provided.";

            public const string MailSubjectAndMessageNotProvided = "Subject and message should be provided.";

            public const string GeneralErrorSendEmail = "Errors occured while sending email.";
        }

        public static class Success
        {
            public const string PasswordResetMailSent = "Password Reset Mail has been sent to your Email {0}.";
        }
    }
}
