namespace GarageBuddy.Services.Data.Services
{
    using System.Threading.Tasks;

    using Contracts;

    public class InstallationService : IInstallationService
    {
        private readonly IUserService userService;

        public InstallationService(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task InstallRequiredDataAsync(string defaultUserEmail, string defaultUserPassword)
        {
            // await AddMigrationAsync();
            await AddDefaultRolesAsync();
            await AddDefaultUserAsync(defaultUserEmail, defaultUserPassword);
        }

        public async Task InstallSampleDataAsync(string defaultUserEmail)
        {
            throw new System.NotImplementedException();
        }

        private async Task AddDefaultUserAsync(string userEmail, string userPassword)
        {
            await this.userService.RegisterWithEmailAsync(userEmail, userPassword);
        }

        private async Task AddDefaultRolesAsync()
        {
            // TODO: Implement this method
            await Task.CompletedTask;
        }
    }
}
