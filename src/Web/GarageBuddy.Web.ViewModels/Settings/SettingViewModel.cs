namespace GarageBuddy.Web.ViewModels.Settings
{
    using System;

    using AutoMapper;

    using Common.Attributes;

    using Data.Models;

    using Services.Mapping;

    public class SettingViewModel : IMapFrom<Setting>, IHaveCustomMappings
    {
        public Guid Id { get; set; }

        [Sanitize]
        public string Name { get; set; } = null!;

        [Sanitize]
        public string Value { get; set; } = null!;

        [Sanitize]
        public string NameAndValue { get; set; } = null!;

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Setting, SettingViewModel>().ForMember(
                m => m.NameAndValue,
                opt => opt.MapFrom(x => x.Name + " = " + x.Value));
        }
    }
}
