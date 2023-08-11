namespace GarageBuddy.Common.Constants
{
    public class ErrorMessageConstants
    {
        public const string ErrorGeneral = "Something went wrong!";

        public const string NoEntityWithPropertyFound = "Entity {0} with property {1} was not found!";

        public const string DeserializationFailed = "Deserialization of the file {0} failed!";

        public const string InvalidDirectoryPath = "Invalid directory path!";

        public const string ErrorCannotBeNullOrWhitespace = "{0} cannot be null or whitespace.";

        public const string ErrorInvalidUsernameOrPassword = "Invalid username or password.";

        public const string ErrorAccountLockedOut = "Account locked out. Try again later";

        public const string ErrorUserNotFound = "User not found.";

        public const string ErrorPasswordsDoNotMatch = "Passwords do not match.";

        public const string ErrorSomethingWentWrong = "Something went wrong. Please try again.";

        public const string ErrorConnectionStringWrongFormat = "Connection string is in wrong format!";

        public const string ErrorDatabaseCreationFailed = "Database creation failed! Error: {0}";

        public const string ErrorDatabaseNotExists = "Database does not exist!";

        public const string ErrorDatabaseInstallationFailed = "Database installation failed! Error: {0}";

        public const string ErrorSendGridApiKeyNotProvided = "SendGrid API key is not provided.";

        public const string ErrorMailSubjectAndMessageNotProvided = "Subject and message should be provided.";

        public const string ErrorGeneralSendEmail = "Error occured while sending email.";
    }
}
