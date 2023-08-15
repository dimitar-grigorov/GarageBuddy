namespace GarageBuddy.Services.Data.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GarageBuddy.Common.Core.Enums;
    using GarageBuddy.Data.Common.Repositories;
    using GarageBuddy.Data.Models;
    using GarageBuddy.Services.Data.Contracts;
    using GarageBuddy.Services.Mapping;

    public class SettingsService : ISettingsService
    {
        private readonly IDeletableEntityRepository<Setting, Guid> settingsRepository;

        public SettingsService(IDeletableEntityRepository<Setting, Guid> settingsRepository)
        {
            this.settingsRepository = settingsRepository;
        }

        public int GetCount()
        {
            return settingsRepository.All(ReadOnlyOption.Normal).Count();
        }

        public IEnumerable<T> GetAll<T>()
        {
            return settingsRepository.All(ReadOnlyOption.Normal).To<T>().ToList();
        }
    }
}
