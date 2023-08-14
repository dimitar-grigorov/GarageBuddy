namespace GarageBuddy.Web.Controllers
{
    using System.Net;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static GarageBuddy.Common.Constants.ControllersAndActionsConstants;

    [Authorize]
    public class BaseController : Controller
    {

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
    }
}
