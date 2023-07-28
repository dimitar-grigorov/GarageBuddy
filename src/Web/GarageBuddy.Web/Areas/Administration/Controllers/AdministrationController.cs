namespace GarageBuddy.Web.Areas.Administration.Controllers
{
    using GarageBuddy.Common.Constants;
    using GarageBuddy.Web.Controllers;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class AdministrationController : BaseController
    {
    }
}
