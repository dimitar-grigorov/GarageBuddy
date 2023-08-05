namespace GarageBuddy.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Data.DataProvider;

    using GarageBuddy.Common.Core;
    using GarageBuddy.Common.Core.Settings;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
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
        private readonly IOptions<DatabaseSettings> connectionStringsOptions;

        public InstallController(
            IOptionsManager optionsManager,
            IWebHelper webHelper,
            IDataProvider dataProvider,
            IInstallationService installationService,
            IOptions<DatabaseSettings> connectionStringsOptions)
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

                // For debugging purposes
                ServerName = "192.168.2.100",
                DatabaseName = "GarageBuddy",
                IntegratedSecurity = false,
                Username = "sa",
                Password = "123456",
                AdminPassword = "123456",
                ConfirmPassword = "123456",
            };

            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public virtual IActionResult Index(InstallFormModel model)
        {
            if (optionsManager.IsDatabaseInstalled())
            {
                return this.RedirectToAction("Index", "Home");
            }

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
                connectionStringsOptions.Value.ConnectionString = connectionString;

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
    }
}
