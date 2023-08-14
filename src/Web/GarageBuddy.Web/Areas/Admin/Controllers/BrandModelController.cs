namespace GarageBuddy.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class BrandModelController : AdminController
    {
        public IActionResult Index(int brandId)
        {
            return View();
        }
    }
}
