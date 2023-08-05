namespace GarageBuddy.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Data.DataProvider;

    using GarageBuddy.Common.Core;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.Extensions.Options;

    using Services.Data.Contracts;
    using Services.Data.Options;

    using ViewModels.Install;

    using static Common.Constants.ErrorMessageConstants;

    public class InstallController : BaseController
    {
        private readonly IOptionsManager optionsManager;
        private readonly IWebHelper webHelper;
        private readonly IDataProvider dataProvider;
        private readonly IInstallationService installationService;
        private readonly IOptions<ConnectionStringsOptions> connectionStringsOptions;

        public InstallController(
            IOptionsManager optionsManager,
            IWebHelper webHelper,
            IDataProvider dataProvider,
            IInstallationService installationService,
            IOptions<ConnectionStringsOptions> connectionStringsOptions)
        {
            this.optionsManager = optionsManager;
            this.webHelper = webHelper;
            this.dataProvider = dataProvider;
            this.installationService = installationService;
            this.connectionStringsOptions = connectionStringsOptions;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            if (optionsManager.IsDatabaseInstalled())
            {
                return this.RedirectToAction("Index", "Home");
            }

            var model = new InstallFormModel()
            {
                AdminEmail = "admin@yourStore.com",
                InstallSampleData = true,
                CreateDatabaseIfNotExists = true,
                ConnectionStringRaw = false,
                DataProvider = DataProviderType.SqlServer,

                // For debugging purposes
                ServerName = "192.168.2.100",
                DatabaseName = "GarageBuddy",
                IntegratedSecurity = false,
                Username = "sa",
                Password = "123456",
                AdminPassword = "123456",
                ConfirmPassword = "123456",
            };

            PrepareAvailableDataProviders(model);
            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public virtual async Task<IActionResult> Index(InstallFormModel model)
        {
            if (optionsManager.IsDatabaseInstalled())
            {
                return this.RedirectToAction("Index", "Home");
            }

            PrepareAvailableDataProviders(model);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var connectionString = model.ConnectionStringRaw
                    ? model.ConnectionString
                    : dataProvider.BuildConnectionString(model);

                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new Exception(ErrorConnectionStringWrongFormat);
                }

                // Save Settings
                connectionStringsOptions.Value.DefaultConnection = connectionString;

                /* if (model.CreateDatabaseIfNotExists)
                {
                    try
                    {
                        dataProvider.Value.CreateDatabase(connectionString);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format(ErrorDatabaseCreationFailed, ex.Message));
                    }
                }
                else
                {
                    // check whether database exists
                    if (!await dataProvider.Value.DatabaseExistsAsync(connectionString))
                    {
                        throw new Exception(ErrorDatabaseNotExists);
                    }
                }

                // TODO: Apply migrations and check if database creation is needed
                await installationService.Value.InstallRequiredDataAsync(model.AdminEmail, model.AdminPassword);

                if (model.InstallSampleData)
                {
                    await installationService.Value.InstallSampleDataAsync(model.AdminEmail);
                }*/

                // return View(new InstallFormModel { RestartUrl = Url.RouteUrl("Homepage")! });
                return RedirectToAction(nameof(RestartApplication));
            }
            catch (Exception exception)
            {
                // TODO: clear provider settings if something got wrong
                ModelState.AddModelError(string.Empty, string.Format(ErrorDatabaseInstallationFailed, exception.Message));
            }

            return View(model);
        }

        [AllowAnonymous]
        public virtual IActionResult RestartApplication()
        {
            if (optionsManager.IsDatabaseInstalled())
            {
                return this.RedirectToAction("Index", "Home");
            }

            // Restart application
            webHelper.RestartAppDomain();

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
