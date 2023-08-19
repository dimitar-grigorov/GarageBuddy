namespace GarageBuddy.Services.Data.Services
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using GarageBuddy.Common.Core.Wrapper;
    using GarageBuddy.Common.Core.Wrapper.Generic;
    using GarageBuddy.Data.Models;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.WebUtilities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    using Models.ApplicationUser;

    public class UserService : IUserService
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly ILogger<UserService> logger;

        public UserService(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            ILogger<UserService> logger)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.logger = logger;
        }

        public async Task<SignInResult> LoginWithUsernameAsync(string username, string password,
            bool isPersistent, bool lockoutOnFailure)
        {
            if (string.IsNullOrWhiteSpace(username.Trim()))
            {
                throw new ArgumentException(string.Format(Errors.CannotBeNullOrWhitespace, "Username"), nameof(username));
            }

            if (string.IsNullOrWhiteSpace(password.Trim()))
            {
                throw new ArgumentException(string.Format(Errors.CannotBeNullOrWhitespace, "Password"), nameof(password));
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
                throw new ArgumentException(string.Format(Errors.CannotBeNullOrWhitespace, "Email"), nameof(email));
            }

            if (string.IsNullOrWhiteSpace(password.Trim()))
            {
                throw new ArgumentException(string.Format(Errors.CannotBeNullOrWhitespace, "Password"), nameof(password));
            }

            var user = await this.userManager.FindByEmailAsync(email);
            var result = await this.signInManager.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);

            return result;
        }

        public async Task<IdentityResult> RegisterWithEmailAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email.Trim()))
            {
                throw new ArgumentException(string.Format(Errors.CannotBeNullOrWhitespace, "Email"), nameof(email));
            }

            if (string.IsNullOrWhiteSpace(password.Trim()))
            {
                throw new ArgumentException(string.Format(Errors.CannotBeNullOrWhitespace, "Password"), nameof(password));
            }

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
            };

            var result = await this.userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await this.signInManager.SignInAsync(user, isPersistent: true);
            }

            // If it is the first user, make him admin
            if (this.userManager.Users.Count() == 1)
            {
                await this.userManager.AddToRoleAsync(user, Roles.Administrator);
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

        public async Task<IResult<string>> GeneratePasswordResetTokenAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                // Don't return more detailed information here to prevent email enumeration
                return await Result<string>.FailAsync(Errors.GeneralError);
            }

            if (userManager.Options.SignIn.RequireConfirmedEmail && !(await userManager.IsEmailConfirmedAsync(user)))
            {
                return await Result<string>.FailAsync(Errors.GeneralError);
            }

            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            return await Result<string>.SuccessAsync(data: token);
        }

        public async Task<IResult<string>> GenerateEmailResetUriAsync(string email, string origin, string route, string tokenQueryKey)
        {
            var tokenResult = await GeneratePasswordResetTokenAsync(email);
            if (!tokenResult.Succeeded)
            {
                return await Result<string>.FailAsync(tokenResult.Messages);
            }

            if (string.IsNullOrWhiteSpace(origin))
            {
                logger.LogError(Errors.CannotBeNullOrWhitespace, nameof(origin));
                return await Result<string>.FailAsync(Errors.GeneralError);
            }

            if (string.IsNullOrWhiteSpace(route))
            {
                logger.LogError(Errors.CannotBeNullOrWhitespace, nameof(route));
                return await Result<string>.FailAsync(Errors.GeneralError);
            }

            if (string.IsNullOrWhiteSpace(tokenQueryKey))
            {
                logger.LogError(Errors.CannotBeNullOrWhitespace, nameof(tokenQueryKey));
                return await Result<string>.FailAsync(Errors.GeneralError);
            }

            var endpointUri = new Uri(string.Concat(origin, route));
            var token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(tokenResult.Data));
            var verificationUri = QueryHelpers.AddQueryString(endpointUri.ToString(), tokenQueryKey, token);
            return await Result<string>.SuccessAsync(data: verificationUri);
        }

        public async Task<IResult> ResetPasswordAsync(string email, string password, string token)
        {
            var user = await userManager.FindByEmailAsync(email.Normalize());
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return await Result.FailAsync(Errors.GeneralError);
            }

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

            var result = await userManager.ResetPasswordAsync(user, decodedToken, password);
            if (result.Succeeded)
            {
                logger.LogInformation($"User with email {email} has reset his password.");
                return await Result.SuccessAsync(Success.PasswordResetSuccessful);
            }

            result.Errors.ToList().ForEach(error => logger.LogError(error.Description));

            return await Result.FailAsync(Errors.GeneralError);
        }

        public async Task<ICollection<UserSelectServiceModel>> GetAllSelectAsync()
        {
            var users = await userManager.Users
                .Select(c => new UserSelectServiceModel
                {
                    Id = c.Id.ToString(),
                    FullName = c.UserName,
                    Email = c.Email,
                })
                .OrderBy(c => c.FullName)
                .ThenBy(c => c.Email)
                .ToListAsync();
            return users;
        }

        public async Task<IEnumerable<UserServiceModel>> GetAllUsersWithRolesAsync()
        {
            var users = await userManager.Users.ToListAsync();
            var userModels = new List<UserServiceModel>();

            foreach (var user in users)
            {
                var userRoles = await userManager.GetRolesAsync(user);
                var userModel = new UserServiceModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    Roles = userRoles,
                };
                userModels.Add(userModel);
            }

            return userModels;
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(Guid userId)
        {
            var user = await this.userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                return new List<string>();
            }

            var roles = await userManager.GetRolesAsync(user);

            return roles;
        }

        public async Task<IEnumerable<string>> GetAllRolesAsync()
        {
            var roles = await this.roleManager.Roles.ToListAsync();
            return roles.Select(r => r.Name);
        }

        public async Task EditAsync(UserServiceModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (!Validate(model))
            {
                throw new ArgumentException(
                    string.Format(Errors.EntityModelStateIsNotValid, "User"),
                    nameof(model));
            }

            var id = model.Id?.ToString() ?? throw new ArgumentNullException(nameof(model.Id));

            var user = await this.userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.Email = model.Email;
            user.UserName = model.UserName;

            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                var token = await this.userManager.GeneratePasswordResetTokenAsync(user);
                await this.userManager.ResetPasswordAsync(user, token, model.Password);
            }

            foreach (var role in model.Roles)
            {
                await this.AddToRoleAsync(user.Id, role);
            }

            foreach (var userRole in await this.GetUserRolesAsync(user.Id))
            {
                if (!model.Roles.Contains(userRole))
                {
                    await this.RemoveFromRoleAsync(user.Id, userRole);
                }
            }

            await this.userManager.UpdateAsync(user);
        }

        public async Task AddToRoleAsync(Guid userId, string role)
        {
            var user = await this.userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return;
            }

            if (await this.roleManager.RoleExistsAsync(role)
                && !await this.userManager.IsInRoleAsync(user, role))
            {
                await this.userManager.AddToRoleAsync(user, role);
            }
        }

        public async Task RemoveFromRoleAsync(Guid userId, string role)
        {
            var user = await this.userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return;
            }

            if (await this.roleManager.RoleExistsAsync(role)
                && await this.userManager.IsInRoleAsync(user, role))
            {
                await this.userManager.RemoveFromRoleAsync(user, role);
            }
        }

        public async Task<bool> ExistsAsync(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);
            return user != null;
        }

        private static bool Validate<TModel>(TModel dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var context = new ValidationContext(dto, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(dto, context, validationResults, true);

            return isValid;
        }
    }
}
