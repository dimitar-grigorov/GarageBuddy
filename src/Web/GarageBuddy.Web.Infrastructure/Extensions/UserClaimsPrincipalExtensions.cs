namespace GarageBuddy.Web.Infrastructure.Extensions
{
    using System;

    using System.Security.Claims;

    using GarageBuddy.Common.Constants;

    /// <summary>
    /// This class holds all extension methods for the user claims principal.
    /// </summary>
    public static class UserClaimsPrincipalExtensions
    {
        /// <summary>
        /// This extension method returns the user's id.
        /// </summary>
        /// <param name="user">The current user.</param>
        /// <returns>Returns a <see cref="Guid"/> representing the user id.</returns>
        public static Guid GetId(this ClaimsPrincipal user)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            return userId == null ? Guid.Empty : Guid.Parse(userId);
        }

        /// <summary>
        /// This extension method returns the user's username.
        /// </summary>
        /// <param name="user">The current user.</param>
        /// <returns>Returns a <see cref="string"/>. The user's username.</returns>
        public static string GetUsername(this ClaimsPrincipal user)
        {
            return user.Identity?.Name ?? string.Empty;
        }

        /// <summary>
        /// If the username is an email, this extension method returns the first part of the email.
        /// For example if the email is "test@user.com" it will return "test".
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Returns a <see cref="string"/> of everything before the symbol @. </returns>
        public static string GetFirstPartOfEmail(this ClaimsPrincipal user)
        {
            var username = user.Identity?.Name ?? string.Empty;
            return username.Contains('@') ? username.Split('@')[0] : username;
        }

        /// <summary>
        /// This extension method determines whether the user is an administrator.
        /// </summary>
        /// <param name="user">The current user.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the user is an admin.</returns>
        public static bool IsAdmin(this ClaimsPrincipal user)
        {
            return user.IsInRole(GlobalConstants.Roles.Administrator);
        }
    }
}
