namespace GarageBuddy.Web.Infrastructure.ViewRenderer
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Abstractions;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.Razor;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewEngines;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.AspNetCore.Routing;

    public class ViewRenderer : IViewRenderer
    {
        private readonly IRazorViewEngine viewEngine;
        private readonly ITempDataProvider tempDataProvider;
        private readonly IServiceProvider serviceProvider;

        public ViewRenderer(
            IRazorViewEngine viewEngine,
            ITempDataProvider tempDataProvider,
            IServiceProvider serviceProvider)
        {
            this.viewEngine = viewEngine;
            this.tempDataProvider = tempDataProvider;
            this.serviceProvider = serviceProvider;
        }

        public async Task<string> RenderAsync<TModel>(TModel model, string viewName, string viewPath = "")
        {
            var actionContext = GetActionContext();

            ViewEngineResult viewEngineResult;

            if (viewPath.Trim() == string.Empty)
            {
                viewEngineResult = viewEngine.FindView(actionContext, viewName, false);
            }
            else
            {
                var (fixedViewPath, fixedViewName) = GetProperViewPathAndName(viewPath, viewName);
                viewEngineResult = viewEngine.GetView(fixedViewPath, fixedViewName, isMainPage: false);
            }

            if (!viewEngineResult.Success)
            {
                throw new InvalidOperationException($"Couldn't find view '{viewName}'");
            }

            var view = viewEngineResult.View;

            await using var output = new StringWriter();

            var viewContext = new ViewContext(
                actionContext,
                view,
                new ViewDataDictionary<TModel>(
                    metadataProvider: new EmptyModelMetadataProvider(),
                    modelState: new ModelStateDictionary())
                {
                    Model = model,
                },
                new TempDataDictionary(
                    actionContext.HttpContext,
                    tempDataProvider),
                output,
                new HtmlHelperOptions());

            await view.RenderAsync(viewContext);

            return output.ToString();
        }

        private static (string, string) GetProperViewPathAndName(string viewPath, string viewName)
        {
            if (!viewName.EndsWith(".cshtml"))
            {
                viewName += ".cshtml";
            }

            if (!viewPath.EndsWith(Path.DirectorySeparatorChar))
            {
                viewPath += Path.DirectorySeparatorChar;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return (viewPath, viewName);
            }

            var executingFilePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
            var mailTemplateViewPath = Path.Combine(viewPath, viewName);
            return (executingFilePath, mailTemplateViewPath);
        }

        private ActionContext GetActionContext()
        {
            var httpContext = new DefaultHttpContext
            {
                RequestServices = serviceProvider,
            };
            var actionContext = new ActionDescriptor();
            return new ActionContext(httpContext, new RouteData(), actionContext);
        }
    }
}
