namespace GarageBuddy.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AutoMapper;

    using GarageBuddy.Data.Common.Repositories;
    using GarageBuddy.Data.Models.Job;
    using GarageBuddy.Tests.Common;

    using Models.Job.JobItemType;

    using NUnit.Framework;

    using Services;

    [TestFixture]
    public class JobItemTypeServiceTests
    {
        private ICollection<JobItemType> jobItemTypes = new List<JobItemType>()
        {
            new()
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                JobTypeName = "Job 1",
                IsDeleted = true,
            },
            new()
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                JobTypeName = "Job 2",
                IsDeleted = false,
            },
            new()
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                JobTypeName = "Job 3",
                IsDeleted = false,
            },
        }.AsReadOnly();

        private IMapper mapper;
        private IDeletableEntityRepository<JobItemType, Guid> repository = null!;

        [SetUp]
        public async Task SetUp()
        {
            this.mapper = MapperMock.Instance;
            this.repository = DeletableEntityRepositoryMock<JobItemType, Guid>.Instance;

            foreach (var item in this.jobItemTypes)
            {
                await this.repository.AddAsync(item);
            }

            await this.repository.SaveChangesAsync();
        }

        [TearDown]
        public void TearDown()
        {
            this.repository.Dispose();
        }

        [Test]
        [Order(1)]
        public async Task GetAllAsync_ShouldReturnAllJobItemTypes()
        {
            TearDown();
            await SetUp();
            var service = new JobItemTypeService(this.repository, this.mapper);

            var result = await service.GetAllAsync();

            Assert.That(result.Count, Is.EqualTo(this.jobItemTypes.Count));
        }

        [Test]
        [Order(2)]
        public async Task GetAllSelectAsync_ShouldReturnAllJobItemTypes()
        {
            var service = new JobItemTypeService(this.repository, this.mapper);

            var result = await service.GetAllSelectAsync();

            Assert.That(result.Count, Is.EqualTo(this.jobItemTypes.Count));
        }

        [Test]
        [Order(3)]
        public async Task EditAsync_ShouldEditJobItemType()
        {
            var service = new JobItemTypeService(this.repository, this.mapper);
            var jobItemType = new JobItemTypeServiceModel()
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                JobTypeName = "Job 1 Edited",
            };

            var result = await service.EditAsync(jobItemType.Id, jobItemType);

            Assert.That(result.Succeeded, Is.EqualTo(true));
        }
    }
}
