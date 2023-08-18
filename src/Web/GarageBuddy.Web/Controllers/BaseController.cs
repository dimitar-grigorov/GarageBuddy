namespace GarageBuddy.Web.Controllers
{
    using System.Net;
    using System.Reflection;

    using Common.Attributes;

    using Ganss.Xss;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static Common.Constants.ControllersAndActionsConstants;

    [Authorize]
    public class BaseController : Controller
    {
        private readonly IHtmlSanitizer sanitizer;

        public BaseController(IHtmlSanitizer sanitizer)
        {
            this.sanitizer = sanitizer;
        }

        /// <summary>
        /// This method redirect to the error view providing an error message and status code.
        /// </summary>
        /// <param name="message">The message of the error.</param>
        /// <param name="statusCode">The  of the error.</param>
        /// <returns>Returns <see cref="IActionResult"/>.</returns>
        [NonAction]
        protected IActionResult ShowError(string message, int? statusCode)
        {
            TempData[NotifyError] = message;

            return RedirectToAction(Actions.Error, Controllers.Home,
                new
                {
                    area = string.Empty,
                    statusCode = statusCode ?? (int)HttpStatusCode.BadRequest,
                });
        }

        /// <summary>
        /// This method sanitizes the properties on a specified model given the <see cref="SanitizeAttribute"/>.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="model">The model to be sanitized.</param>
        /// <exception cref="ArgumentNullException">Throws when the model is null.</exception>
        [NonAction]
        protected void SanitizeModel<TModel>(TModel model)
        {
            ArgumentNullException.ThrowIfNull(model, nameof(model));

            var type = model.GetType();
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                var shouldBeSanitized = property.IsDefined(typeof(SanitizeAttribute), false);

                if (property.PropertyType == typeof(string) && shouldBeSanitized)
                {
                    if (property.GetValue(model) is string value)
                    {
                        property.SetValue(model, this.sanitizer.Sanitize(value));
                    }
                }
            }
        }
    }
}
