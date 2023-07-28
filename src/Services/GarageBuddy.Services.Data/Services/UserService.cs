namespace GarageBuddy.Services.Data.Services
{
    using System;
    using System.Threading.Tasks;

    using Contracts;

    using GarageBuddy.Data.Models;

    using Microsoft.AspNetCore.Identity;

    using static Common.Constants.ErrorMessageConstants;

    public class UserService : IUserService
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;

        public UserService(SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        public async Task<SignInResult> LoginWithUsernameAsync(string username, string password,
            bool isPersistent, bool lockoutOnFailure)
        {
            if (string.IsNullOrWhiteSpace(username.Trim()))
            {
                throw new ArgumentException(String.Format(ErrorCannotBeNullOrWhitespace, "Username"), nameof(username));
            }

            if (string.IsNullOrWhiteSpace(password.Trim()))
            {
                throw new ArgumentException(String.Format(ErrorCannotBeNullOrWhitespace, "Password"), nameof(password));
            }

            var user = await this.userManager.FindByNameAsync(username);
            var result = await this.signInManager.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);
            return result;
        }
    }
}
