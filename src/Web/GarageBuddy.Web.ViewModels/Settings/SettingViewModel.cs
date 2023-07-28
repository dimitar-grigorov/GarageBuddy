namespace GarageBuddy.Web.ViewModels.Settings
{
    using System;

    using AutoMapper;

    using GarageBuddy.Data.Models;
    using GarageBuddy.Services.Mapping;

    public class SettingViewModel : IMapFrom<Setting>, IHaveCustomMappings
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string Value { get; set; } = null!;

        public string NameAndValue { get; set; } = null!;

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Setting, SettingViewModel>().ForMember(
                m => m.NameAndValue,
                opt => opt.MapFrom(x => x.Name + " = " + x.Value));
        }
    }
}
