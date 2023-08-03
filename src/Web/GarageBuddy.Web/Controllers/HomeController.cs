namespace GarageBuddy.Web.Controllers
{
    using System.Diagnostics;

    using Common.Constants;

    using Humanizer;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.WebUtilities;

    using ViewModels;

    public class HomeController : BaseController
    {
        [AllowAnonymous]
        public IActionResult Index()
        {
            return this.View();
        }

        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return this.View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? statusCode = null)
        {
            // TODO: Log errors
            var model = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier,
                Title = "System Error",
                Message = "An error occurred while processing your request.",
                ImageUrl = GlobalConstants.ThemeErrorImagesPathTemplate.FormatWith(500),
            };

            if (statusCode.HasValue)
            {
                model.StatusCode = statusCode.Value;
                model.Title = ReasonPhrases.GetReasonPhrase(statusCode.Value);

                switch (statusCode)
                {
                    case 403:
                        model.ImageUrl = GlobalConstants.ThemeErrorImagesPathTemplate.FormatWith(statusCode.Value);
                        model.Message = "You are unauthorized to see this page.";
                        break;
                    case 404:
                        model.ImageUrl = GlobalConstants.ThemeErrorImagesPathTemplate.FormatWith(statusCode.Value);
                        model.Message = "The page you are looking not found.";
                        break;
                }
            }

            return this.View(model);
        }
    }
}
