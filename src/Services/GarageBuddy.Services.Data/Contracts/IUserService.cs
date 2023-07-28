namespace GarageBuddy.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;

    public interface IUserService
    {
        public Task<SignInResult> LoginWithUsernameAsync(string username, string password,
            bool isPersistent, bool lockoutOnFailure);
    }
}
