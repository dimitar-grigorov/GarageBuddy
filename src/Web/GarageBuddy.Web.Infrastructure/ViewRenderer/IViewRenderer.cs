namespace GarageBuddy.Web.Infrastructure.ViewRenderer
{
    using System.Threading.Tasks;

    public interface IViewRenderer
    {
        public Task<string> RenderAsync<TModel>(string viewName, TModel model);
    }
}
