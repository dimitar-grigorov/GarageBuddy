namespace GarageBuddy.Web.ViewModels.Install
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Common.Core;

    using Microsoft.AspNetCore.Mvc.Rendering;

    // TODO: Check validation
    public class InstallFormModel : IConnectionStringInfo
    {
        public InstallFormModel()
        {
            AvailableDataProviders = new List<SelectListItem>();
        }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string AdminEmail { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string AdminPassword { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = null!;

        public bool CreateDatabaseIfNotExists { get; set; }

        public bool InstallSampleData { get; set; }

        public bool ConnectionStringRaw { get; set; }

        public string DatabaseName { get; set; } = null!;

        public string ServerName { get; set; } = null!;

        public bool IntegratedSecurity { get; set; }

        public string Username { get; set; } = null!;

        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        public string? ConnectionString { get; set; }

        public DataProviderType DataProvider { get; set; }

        public List<SelectListItem> AvailableDataProviders { get; set; }

        public string? RestartUrl { get; set; }
    }
}
