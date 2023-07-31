namespace GarageBuddy.Web.Areas.Admin.Controllers
{
    using GarageBuddy.Common.Constants;
    using GarageBuddy.Web.Controllers;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Admin")]
    public class AdminController : BaseController
    {
    }
}
