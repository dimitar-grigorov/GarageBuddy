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

        public async Task<SignInResult> LoginWithEmailAsync(string email, string password, bool isPersistent,
            bool lockoutOnFailure)
        {
            if (string.IsNullOrWhiteSpace(email.Trim()))
            {
                throw new ArgumentException(String.Format(ErrorCannotBeNullOrWhitespace, "Email"), nameof(email));
            }

            if (string.IsNullOrWhiteSpace(password.Trim()))
            {
                throw new ArgumentException(String.Format(ErrorCannotBeNullOrWhitespace, "Password"), nameof(password));
            }

            var user = await this.userManager.FindByEmailAsync(email);
            var result = await this.signInManager.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);

            return result;
        }

        public async Task<IdentityResult> RegisterWithEmailAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email.Trim()))
            {
                throw new ArgumentException(String.Format(ErrorCannotBeNullOrWhitespace, "Email"), nameof(email));
            }

            if (string.IsNullOrWhiteSpace(password.Trim()))
            {
                throw new ArgumentException(String.Format(ErrorCannotBeNullOrWhitespace, "Password"), nameof(password));
            }

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
            };

            var result = await this.userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await this.signInManager.SignInAsync(user, isPersistent: false);
            }

            return result;
        }

        public async Task LogoutAsync()
        {
            await signInManager.SignOutAsync();
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            var user = await this.userManager.FindByIdAsync(id.ToString());
            return user != null;
        }

        public async Task<string> GeneratePasswordResetTokenAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email) ??
                       throw new NullReferenceException(ErrorUserNotFound);
            var passwordResetToken = await userManager.GeneratePasswordResetTokenAsync(user);

            return passwordResetToken;
        }
    }
}
