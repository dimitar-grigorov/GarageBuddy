namespace GarageBuddy.Tests.Common
{
    using System.Reflection;

    using AutoMapper;

    using Services.Data.Models.Vehicle.BrandModel;
    using Services.Mapping;

    /// <summary>
    /// This class is a mock of <see cref="IMapper"/>.
    /// </summary>
    public static class MapperMock
    {
        /// <summary>
        /// Gets the <see cref="IMapper"/> instance.
        /// </summary>
        public static IMapper Instance
        {
            get
            {
                var assemblies = new Assembly[]
                {
                    typeof(BrandModelListServiceModel).GetTypeInfo().Assembly,
                };
                AutoMapperConfig.RegisterMappings(assemblies);

                var mapper = AutoMapperConfig.MapperInstance;
                return mapper;
            }
        }
    }
}
