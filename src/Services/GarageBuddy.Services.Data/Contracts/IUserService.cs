namespace GarageBuddy.Services.Data.Contracts
{
    using Microsoft.AspNetCore.Identity;

    using Models.ApplicationUser;

    public interface IUserService
    {
        public Task<SignInResult> LoginWithUsernameAsync(string username, string password,
            bool isPersistent, bool lockoutOnFailure);

        public Task<SignInResult> LoginWithEmailAsync(string email, string password,
            bool isPersistent, bool lockoutOnFailure);

        public Task<IdentityResult> RegisterWithEmailAsync(string email, string password);

        public Task LogoutAsync();

        public Task<bool> ExistsAsync(Guid id);

        public Task<IResult<string>> GeneratePasswordResetTokenAsync(string email);

        public Task<IResult<string>> GenerateEmailResetUriAsync(string email, string origin, string route, string tokenQueryKey);

        public Task<IResult> ResetPasswordAsync(string email, string password, string token);

        public Task<ICollection<UserSelectServiceModel>> GetAllSelectAsync();

        // public Task<TUser> FindUserByIdAsync(string id);
        // public Task<IEnumerable<TUser>> GetAllUsersAsync();
        public Task<IEnumerable<UserServiceModel>> GetAllUsersWithRolesAsync();

        public Task<IEnumerable<string>> GetUserRolesAsync(Guid userId);

        public Task<IEnumerable<string>> GetAllRolesAsync();

        // public Task EditAsync(UserDto userDto);
        public Task AddToRoleAsync(Guid userId, string role);

        public Task RemoveFromRoleAsync(Guid userId, string role);

        public Task<bool> ExistsAsync(string id);
    }
}
