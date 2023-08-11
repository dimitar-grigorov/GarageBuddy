namespace GarageBuddy.Web.Infrastructure.Extensions
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewEngines;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;

    public static class ControllerExtensions
    {
        public static async Task<string> RenderRazorViewToString(this Controller controller, string viewName, object model = null)
        {
            controller.ViewData.Model = model;

            await using var sw = new StringWriter();

            if (controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) is not ICompositeViewEngine viewEngine)
            {
                throw new Exception("The view engine does not exist");
            }

            var viewResult = viewEngine.FindView(controller.ControllerContext, viewName, false);
            if (viewResult.View == null)
            {
                throw new Exception($"The view '{viewName}' could not be found");
            }

            var viewContext = new ViewContext(
                controller.ControllerContext,
                viewResult.View,
                controller.ViewData,
                controller.TempData,
                sw,
                new HtmlHelperOptions()
            );

            await viewResult.View.RenderAsync(viewContext);
            return sw.GetStringBuilder().ToString();
        }
    }
}
