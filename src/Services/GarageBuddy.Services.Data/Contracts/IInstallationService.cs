namespace GarageBuddy.Services.Data.Contracts
{
    using System.Threading.Tasks;

    /// <summary>
    /// Installation service.
    /// </summary>
    public interface IInstallationService
    {
        /// <summary>
        /// Install required data.
        /// </summary>
        /// <param name="defaultUserEmail">Default user email.</param>
        /// <param name="defaultUserPassword">Default user password.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task InstallRequiredDataAsync(string defaultUserEmail, string defaultUserPassword);

        /// <summary>
        /// Install sample data.
        /// </summary>
        /// <param name="defaultUserEmail">Default user email.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task InstallSampleDataAsync(string defaultUserEmail);
    }
}
