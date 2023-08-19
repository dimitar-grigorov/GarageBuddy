namespace GarageBuddy.Tests.Common
{
    using AutoMapper;

    using Data.Common.Repositories;
    using Data.Models.Vehicle;

    using Moq;

    using Services.Data.Contracts;

    public static class BrandServiceMock
    {
        private static readonly ICollection<Brand> Brands = new List<Brand>()
        {
            new ()
            {
                Id = Guid.Parse("2bfff802-5afb-4bbb-96b3-27c98161ff00"),
                BrandName = "Brand 1",
                IsDeleted = false,
            },
            new ()
            {
                Id = Guid.Parse("9983bb6a-1c02-4d43-b9ba-cf3723d0932a"),
                BrandName = "Brand 2",
                IsDeleted = true,
            },
            new ()
            {
                Id = Guid.Parse("e94ed14e-ad6e-4a31-a362-047c7eca1b19"),
                BrandName = "Brand 3",
                IsDeleted = false,
            },
        }.AsReadOnly();

        private static readonly IDeletableEntityRepository<Brand, Guid> BrandRepository
            = DeletableEntityRepositoryMock<Brand, Guid>.Instance;

        private static IMapper mapper = MapperMock.Instance;

        private static readonly bool IsInitialized = false;

        public static IBrandService Instance
        {
            get
            {
                if (!IsInitialized)
                {
                    foreach (var item in Brands)
                    {
                        BrandRepository.Add(item);
                    }
                }

                var mock = new Mock<IBrandService>();

                mock.Setup(m => m.ExistsAsync(It.IsAny<Guid>()).Result)
                    .Returns((Guid id) =>
                    {
                        return BrandRepository.ExistsAsync(id).GetAwaiter().GetResult();
                    });

                var service = mock.Object;
                return service;
            }
        }
    }
}
