namespace GarageBuddy.Web.Controllers
{
    using System.Diagnostics;

    using Common.Constants;

    using Humanizer;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.WebUtilities;
    using Microsoft.Extensions.Logging;

    using ViewModels;

    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> logger;

        public HomeController(ILogger<HomeController> logger)
        {
            this.logger = logger;
        }

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
            var model = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier,
                Title = "System Errors",
                Message = "An error occurred while processing your request.",
                ImageUrl = GlobalConstants.ThemeErrorImagesPathTemplate.FormatWith(500),
            };

            // Log errors
            logger.LogError($"Error occurred with status code: {statusCode}. Request id: {model.RequestId}");

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
