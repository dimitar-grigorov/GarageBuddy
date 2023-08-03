namespace GarageBuddy.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using GarageBuddy.Common.Core;
    using GarageBuddy.Services.Data.Settings;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    using ViewModels.Install;

    public class InstallController : BaseController
    {
        private readonly ISettingsManager settingsManager;
        private readonly Lazy<IWebHelper> webHelper;

        public InstallController(ISettingsManager settingsManager,
            Lazy<IWebHelper> webHelper)
        {
            this.settingsManager = settingsManager;
            this.webHelper = webHelper;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            if (settingsManager.IsDatabaseInstalled())
            {
                return this.RedirectToAction("Index", "Home");
            }

            var model = new InstallFormModel()
            {
                AdminEmail = "admin@yourStore.com",
                InstallSampleData = false,
                CreateDatabaseIfNotExists = false,
                ConnectionStringRaw = false,
                DataProvider = DataProviderType.SqlServer,
            };

            PrepareAvailableDataProviders(model);
            return View(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> Index(InstallFormModel model)
        {
            if (settingsManager.IsDatabaseInstalled())
            {
                return this.RedirectToAction("Index", "Home");
            }

            PrepareAvailableDataProviders(model);

            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        [AllowAnonymous]
        public virtual IActionResult RestartApplication()
        {
            if (settingsManager.IsDatabaseInstalled())
            {
                return this.RedirectToAction("Index", "Home");
            }

            // Restart application
            webHelper.Value.RestartAppDomain();

            return new EmptyResult();
        }

        protected virtual void PrepareAvailableDataProviders(InstallFormModel model)
        {
            var list = Enum.GetValues(typeof(DataProviderType))
                .Cast<DataProviderType>()
                .Where(e => e != DataProviderType.Unknown)
                .Select(e => new SelectListItem
                {
                    Text = e.ToString(),
                    Value = Convert.ToInt32(e).ToString(),
                })
                .ToList();
            model.AvailableDataProviders.AddRange(list);
        }
    }
}
