namespace GarageBuddy.Common.Constants
{
    public static class MessageConstants
    {
        public const string NotDeleted = "Not deleted";

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

            public const string SomethingWentWrong = "Something went wrong. Please try again later.";

            public const string ConnectionStringWrongFormat = "Connection string is in wrong format!";

            public const string DatabaseCreationFailed = "Database creation failed! Errors: {0}";

            public const string DatabaseNotExists = "Database does not exist!";

            public const string DatabaseInstallationFailed = "Database installation failed! Errors: {0}";

            public const string SendGridApiKeyNotProvided = "SendGrid API key is not provided.";

            public const string MailSubjectAndMessageNotProvided = "Subject and message should be provided.";

            public const string GeneralErrorSendEmail = "Errors occured while sending email.";

            public const string EntitysModelStateIsNotValid = "{0} model state is not valid.";

            public const string EntityNotFound = "The {0} cannot be found.";

            public const string EntityNotCreated = "The {0} cannot be created.";

            public const string EntityWithTheSameNameAlreadyExists = "The {0} with the same name ({1}) already exists.";

            public const string InvalidEntityName = "Invalid {0}.";

            public const string InvalidCoordinates = "Invalid coordinates.";

            public const string SourceOrDestinationNull = "Source or/and Destination Objects are null";

            public const string NoMoreThanOneActiveGarage = "There can be only one active garage. Deactivate the other one.";

            public const string NoValidGarageCoordinates = "No valid garage coordinates found.";
        }

        public static class Success
        {
            public const string PasswordResetMailSent = "Password Reset Mail has been sent to your Email {0}.";

            public const string SendPasswordResetEmail =
                "Please check your email for a link to reset your password. If it doesn't appear within a few minutes, check your spam folder.";

            public const string PasswordResetSuccessful = "Password Reset Successful!";

            public const string SuccessfullyCreatedEntity = "Successfully created {0}.";

            public const string SuccessfullyEditedEntity = "Successfully edited {0}.";
        }
    }
}
