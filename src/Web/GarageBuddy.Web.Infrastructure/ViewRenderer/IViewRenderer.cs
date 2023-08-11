namespace GarageBuddy.Web.Infrastructure.ViewRenderer
{
    using System.Threading.Tasks;

    public interface IViewRenderer
    {
        public Task<string> RenderAsync<TModel>(TModel model, string viewName, string viewPath = "");
    }
}
