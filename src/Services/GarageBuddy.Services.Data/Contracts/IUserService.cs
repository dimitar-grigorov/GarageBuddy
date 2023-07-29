namespace GarageBuddy.Services.Data.Contracts
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;

    public interface IUserService
    {
        // TODO: Check if this is needed
        public Task<SignInResult> LoginWithUsernameAsync(string username, string password,
            bool isPersistent, bool lockoutOnFailure);

        public Task<SignInResult> LoginWithEmailAsync(string email, string password,
            bool isPersistent, bool lockoutOnFailure);

        public Task<IdentityResult> RegisterWithEmailAsync(string email, string password);

        public Task LogoutAsync();

        public Task<bool> ExistsAsync(Guid id);

        public Task<string> GeneratePasswordResetTokenAsync(string email);

        /*
        public Task<TUser> FindUserByIdAsync(string id);

        public Task<IEnumerable<TUser>> GetAllUsersAsync();

        public Task<IEnumerable<string>> GetUserRolesAsync(TUser user);

        public Task<IEnumerable<string>> GetAllRolesAsync();

        public Task EditAsync(UserDto userDto);

        public Task AddToRoleAsync(TUser user, string role);

        public Task RemoveFromRoleAsync(TUser user, string role);
         */
    }
}
