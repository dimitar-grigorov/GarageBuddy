namespace GarageBuddy.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using GarageBuddy.Common.Constants;
    using GarageBuddy.Common.Core.Enums;
    using GarageBuddy.Tests.Common;

    using Microsoft.EntityFrameworkCore;

    using Models.Vehicle;

    using NUnit.Framework;

    using Repositories;

    [TestFixture]
    public class EfDeletableRepositoryTests
    {
        private readonly ICollection<Brand> brands = new List<Brand>
        {
            new Brand()
            {
                Id = Guid.Parse("d2a8b7a1-2e19-4a8c-838e-b512d834241e"),
                BrandName = "Brand 1",
                IsDeleted = false,
            },
            new Brand()
            {
                Id = Guid.Parse("99a27f29-97ee-4847-b8e8-5d8cb969c724"),
                BrandName = "Brand 2",
                IsDeleted = true,
            },
            new Brand()
            {
                Id = Guid.Parse("449b6ca2-3640-4f35-a37a-986f9df4ab7b"),
                BrandName = "Brand 3",
                IsDeleted = false,
            },
        }.AsReadOnly();

        private ApplicationDbContext dbContext = null!;

        [SetUp]
        public async Task Setup()
        {
            this.dbContext = await DbContextMock.InitializeDbContextAsync();
            await this.dbContext.AddRangeAsync(this.brands);
            await this.dbContext.SaveChangesAsync();
        }

        [TearDown]
        public async Task TearDown()
        {
            await DbContextMock.DisposeAsync();
        }

        [Test]
        [Order(1)]
        public async Task AllShouldReturnAllWithoutTracking()
        {
            // Arrange
            var repository = new EfRepository<Brand, Guid>(this.dbContext);

            // Act
            var entities = await repository.All(ReadOnlyOption.ReadOnly).ToListAsync();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(entities, Is.Not.Empty, "No entities were retrieved.");
                Assert.That(entities, Has.Exactly(this.dbContext.Brands.Count()).Items, "Entities count is not correct.");

                Assert.That(
                    entities.Select(e => e.Id),
                    Is.EquivalentTo(this.dbContext.Brands.Select(rn => rn.Id)),
                    "Entities have incorrect ids.");

                Assert.That(
                    entities.Select(e => e.BrandName),
                    Is.EquivalentTo(this.dbContext.Brands.Select(rn => rn.BrandName)),
                    "Entities have incorrect data.");

                Assert.That(entities, Has.All.Matches<Brand>(rn =>
                {
                    var entityState = this.dbContext.Entry(rn).State;

                    return entityState == EntityState.Detached;
                }));
            });
        }

        [Test]
        [Order(2)]
        public async Task AllShouldReturnAllWithTracking()
        {
            // Arrange
            var repository = new EfRepository<Brand, Guid>(this.dbContext);

            // Act
            var entities = await repository.All(ReadOnlyOption.Normal).ToListAsync();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(entities, Is.Not.Empty, "No entities were retrieved.");
                Assert.That(entities, Has.Exactly(this.dbContext.Brands.Count()).Items, "Entities count is not correct.");

                Assert.That(
                    entities.Select(e => e.Id),
                    Is.EquivalentTo(this.dbContext.Brands.Select(rn => rn.Id)),
                    "Entities have incorrect ids.");

                Assert.That(
                    entities.Select(e => e.BrandName),
                    Is.EquivalentTo(this.dbContext.Brands.Select(rn => rn.BrandName)),
                    "Entities have incorrect data.");
            });
        }

        [Test]
        public async Task AllShouldReturnAnEmptyCollection()
        {
            // Arrange
            var repository = new EfRepository<Brand, Guid>(this.dbContext);

            // Act
            var entities = await repository.All(rn => rn.BrandName == "No brand with this body should exist.",
                ReadOnlyOption.Normal).ToListAsync();

            // Assert
            Assert.That(entities, Is.Empty, "The collection contains entities.");
        }

        [Test]
        [TestCase("d2a8b7a1-2e19-4a8c-838e-b512d834241e", "Brand 1")]
        [TestCase("449b6ca2-3640-4f35-a37a-986f9df4ab7b", "Brand 3")]
        public async Task AllShouldReturnAllWithTrackingFiltered(string id, string brandName)
        {
            // Arrange
            var repository = new EfRepository<Brand, Guid>(this.dbContext);

            // Act
            var entities = await repository.All(rn => rn.BrandName == brandName, ReadOnlyOption.Normal).ToListAsync();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(entities, Is.Not.Empty, "No entities were retrieved.");
                Assert.That(entities, Has.Exactly(1).Items, "Only one entity should be returned.");
                Assert.That(entities[0].Id.ToString(), Is.EqualTo(id), "The entity's id is not correct.");
                Assert.That(entities[0].BrandName, Is.EqualTo(brandName), "The entity's data is not correct.");
                Assert.That(this.dbContext.Entry(entities[0]).State, Is.Not.EqualTo(EntityState.Detached), "Entity's state is not tracked.");
            });
        }

        [Test]
        [TestCase("d2a8b7a1-2e19-4a8c-838e-b512d834241e", "Brand 1")]
        [TestCase("449b6ca2-3640-4f35-a37a-986f9df4ab7b", "Brand 3")]
        public async Task AllShouldReturnAllWithoutTrackingFiltered(string id, string brandName)
        {
            // Arrange
            var repository = new EfRepository<Brand, Guid>(this.dbContext);

            // Act
            var entities = await repository.All(rn => rn.BrandName == brandName, ReadOnlyOption.ReadOnly).ToListAsync();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(entities, Is.Not.Empty, "No entities were retrieved.");
                Assert.That(entities, Has.Exactly(1).Items, "Only one entity should be returned.");
                Assert.That(entities[0].Id.ToString(), Is.EqualTo(id), "The entity's id is not correct.");
                Assert.That(entities[0].BrandName, Is.EqualTo(brandName), "The entity's data is not correct.");
                Assert.That(this.dbContext.Entry(entities[0]).State, Is.EqualTo(EntityState.Detached), "Entity's state is not detached.");
            });
        }

        [Test]
        [TestCase("d2a8b7a1-2e19-4a8c-838e-b512d834241e", "Brand 1")]
        [TestCase("449b6ca2-3640-4f35-a37a-986f9df4ab7b", "Brand 3")]
        public async Task FindAsyncShouldReturnEntityWithIdWithoutTracking(string id, string brandName)
        {
            // Arrange
            var repository = new EfRepository<Brand, Guid>(this.dbContext);

            // Act
            var entity = await repository.FindAsync(Guid.Parse(id), ReadOnlyOption.ReadOnly);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(entity.Id.ToString(), Is.EqualTo(id), "Entity's id is incorrect.");
                Assert.That(entity.BrandName, Is.EqualTo(brandName), "Entity's data is incorrect.");
                Assert.That(this.dbContext.Entry(entity).State, Is.EqualTo(EntityState.Detached), "Entity's state is not detached.");
            });
        }

        [Test]
        [TestCase("d2a8b7a1-2e19-4a8c-838e-b512d834241e", "Brand 1")]
        [TestCase("449b6ca2-3640-4f35-a37a-986f9df4ab7b", "Brand 3")]
        public async Task FindAsyncShouldReturnEntityWithIdWithTracking(string id, string brandName)
        {
            // Arrange
            var repository = new EfRepository<Brand, Guid>(this.dbContext);

            // Act
            var entity = await repository.FindAsync(Guid.Parse(id), ReadOnlyOption.Normal);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(entity.Id.ToString(), Is.EqualTo(id), "Entity's id is incorrect.");
                Assert.That(entity.BrandName, Is.EqualTo(brandName), "Entity's data is incorrect.");
                Assert.That(this.dbContext.Entry(entity).State, Is.Not.EqualTo(EntityState.Detached), "Entity's state is not tracked.");
            });
        }

        [Test]
        [TestCase("022f6770-dd45-4c02-b9ed-529134a85595")]
        public void FindAsyncShouldThrowWhenEntityDoesNotExist(string id)
        {
            // Arrange
            var repository = new EfRepository<Brand, Guid>(this.dbContext);

            // Act
            async Task Code() => await repository.FindAsync(Guid.Parse(id), ReadOnlyOption.Normal);

            // Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await Code(), "Invalid operation exception should be thrown.");
        }

        [Test]
        [TestCase("I was added by Add() method.")]
        public async Task AddShouldAddEntityToDatabaseAndReturnEntityEntry(string brandName)
        {
            // Arrange
            var repository = new EfRepository<Brand, Guid>(this.dbContext);

            // Act
            var result = await repository.AddAsync(new Brand() { BrandName = brandName });
            await repository.SaveChangesAsync();

            // Assert
            var entity = await this.dbContext.Brands.FindAsync(result.Entity.Id)
                ?? throw new InvalidOperationException(string.Format(MessageConstants.Errors.EntityNotFound, "entity"));
            Assert.Multiple(() =>
            {
                Assert.That(entity, Is.Not.Null, "The entity cannot be found.");
                Assert.That(entity.CreatedOn, Is.Not.EqualTo(default(DateTime)), "Created should be set.");
                Assert.That(result.Entity.Id, Is.EqualTo(entity.Id), "Ids don't match.");
                Assert.That(brandName, Is.EqualTo(entity.BrandName), "Entity data is not set or is not matching.");
            });
        }

        [Test]
        [TestCase("I was added by AddAsync() method.")]
        public async Task AddAsyncShouldAddEntityToDatabaseAndReturnEntityEntry(string brandName)
        {
            // Arrange
            var repository = new EfRepository<Brand, Guid>(this.dbContext);

            // Act
            var result = await repository.AddAsync(new Brand() { BrandName = brandName });
            await repository.SaveChangesAsync();

            // Assert
            var entity = await this.dbContext.Brands.FindAsync(result.Entity.Id)
                ?? throw new InvalidOperationException(string.Format(MessageConstants.Errors.EntityNotFound, "entity"));
            Assert.Multiple(() =>
            {
                Assert.That(entity, Is.Not.Null, "The entity cannot be found.");
                Assert.That(entity.CreatedOn, Is.Not.EqualTo(default(DateTime)), "Created should be set.");
                Assert.That(result.Entity.Id, Is.EqualTo(entity.Id), "Ids don't match.");
                Assert.That(brandName, Is.EqualTo(entity.BrandName), "Entity data is not set or is not matching.");
            });
        }

        [Test]
        [TestCase("I was added by AddRange() method.", "Me too.")]
        public async Task AddRangeShouldAddEntitiesToDatabase(string brandName1, string brandName2)
        {
            // Arrange
            var repository = new EfRepository<Brand, Guid>(this.dbContext);
            var brandList = new List<Brand>()
            {
                new() { BrandName = brandName1 },
                new() { BrandName = brandName2 },
            };

            // Act
            await repository.AddRangeAsync(brandList);
            await repository.SaveChangesAsync();

            // Assert
            var entity1 = await dbContext
                .Brands
                .FirstAsync(rn => rn.BrandName == brandName1);
            Assert.Multiple(() =>
            {
                Assert.That(entity1, Is.Not.Null, "The entity cannot be found.");
                Assert.That(entity1.CreatedOn, Is.Not.EqualTo(default(DateTime)), "Created should be set.");
                Assert.That(brandName1, Is.EqualTo(entity1.BrandName), "Entity data is not set or is not matching.");
            });

            var entity2 = await dbContext
                .Brands
                .FirstAsync(rn => rn.BrandName == brandName2);

            Assert.Multiple(() =>
            {
                Assert.That(entity2, Is.Not.Null, "The entity cannot be found.");
                Assert.That(entity2.CreatedOn, Is.Not.EqualTo(default(DateTime)), "Created should be set.");
                Assert.That(brandName2, Is.EqualTo(entity2.BrandName), "Entity data is not set or is not matching.");
            });
        }

        [Test]
        [TestCase("I was added by AddRange() method.", "Me too.")]
        public async Task AddRangeAsyncShouldAddEntitiesToDatabase(string brandName1, string brandName2)
        {
            // Arrange
            var repository = new EfRepository<Brand, Guid>(dbContext);
            var brandList = new List<Brand>()
            {
                new () { BrandName = brandName1 },
                new () { BrandName = brandName2 },
            };

            // Act
            await repository.AddRangeAsync(brandList);
            await repository.SaveChangesAsync();

            // Assert
            var entity1 = await this.dbContext
                .Brands
                .FirstAsync(rn => rn.BrandName == brandName1);
            Assert.Multiple(() =>
            {
                Assert.That(entity1, Is.Not.Null, "The entity cannot be found.");
                Assert.That(entity1.CreatedOn, Is.Not.EqualTo(default(DateTime)), "Created should be set.");
                Assert.That(brandName1, Is.EqualTo(entity1.BrandName), "Entity data is not set or is not matching.");
            });

            var entity2 = await this.dbContext
                .Brands
                .FirstAsync(rn => rn.BrandName == brandName2);
            Assert.Multiple(() =>
            {
                Assert.That(entity2, Is.Not.Null, "The entity cannot be found.");
                Assert.That(entity2.CreatedOn, Is.Not.EqualTo(default(DateTime)), "Created should be set.");
                Assert.That(brandName2, Is.EqualTo(entity2.BrandName), "Entity data is not set or is not matching.");
            });
        }

        [Test]
        [TestCase("a5652bde-19cd-4ce9-88ff-daf3f7bfd3eb", "My body does not matter because it will be updated.", "I told you ;)")]
        public async Task UpdateShouldUpdateEntity(string id, string brandName, string modifiedBody)
        {
            // Arrange
            var repository = new EfRepository<Brand, Guid>(this.dbContext);
            var guid = Guid.Parse(id);
            await this.dbContext.AddAsync(new Brand() { Id = guid, BrandName = brandName });
            await this.dbContext.SaveChangesAsync();

            // Act
            var entity = await this.dbContext.Brands.FindAsync(guid)
                ?? throw new InvalidOperationException(string.Format(MessageConstants.Errors.EntityNotFound, "entity"));
            entity.BrandName = modifiedBody;
            repository.Update(entity);

            // Assert
            Assert.That(this.dbContext.Entry(entity).State, Is.EqualTo(EntityState.Modified), "Entity's state should be Modified.");
            await repository.SaveChangesAsync();

            entity = await this.dbContext.Brands.FindAsync(guid)
                ?? throw new InvalidOperationException(string.Format(MessageConstants.Errors.EntityNotFound, "entity"));
            Assert.Multiple(() =>
            {
                Assert.That(entity, Is.Not.Null, "Entity cannot be found.");
                Assert.That(entity.BrandName, Is.EqualTo(modifiedBody), "Entity's data is not updated.");
                Assert.That(entity.ModifiedOn, Is.Not.Null, "ModifiedOn should not be null.");
            });
        }

        [Test]
        [TestCase("67a55fdd-269d-474e-a0e8-cba4cbb5f24c", "My body does not matter because it will be updated.", "I told you ;)")]
        public async Task UpdateShouldTrackAndUpdateEntity(string id, string brandName, string modifiedBody)
        {
            // Arrange
            var repository = new EfRepository<Brand, Guid>(this.dbContext);
            var guid = Guid.Parse(id);
            await this.dbContext.AddAsync(new Brand() { Id = guid, BrandName = brandName });
            await this.dbContext.SaveChangesAsync();

            // Act
            var entity = await this.dbContext.Brands.FindAsync(guid) ??
                throw new InvalidOperationException(string.Format(MessageConstants.Errors.NoEntityWithPropertyFound, "entity", nameof(id)));

            this.dbContext.Entry(entity).State = EntityState.Detached;

            entity.BrandName = modifiedBody;
            repository.Update(entity);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(this.dbContext.Entry(entity).State, Is.Not.EqualTo(EntityState.Detached), "Entity should be tracked.");
                Assert.That(this.dbContext.Entry(entity).State, Is.EqualTo(EntityState.Modified), "Entity's state should be Modified.");
            });
            await repository.SaveChangesAsync();

            entity = await this.dbContext.Brands.FindAsync(guid)
                ?? throw new InvalidOperationException(string.Format(MessageConstants.Errors.EntityNotFound, "entity"));
            Assert.Multiple(() =>
            {
                Assert.That(entity, Is.Not.Null, "Entity cannot be found.");
                Assert.That(entity.BrandName, Is.EqualTo(modifiedBody), "Entity's data is not updated.");
                Assert.That(entity.ModifiedOn, Is.Not.Null, "ModifiedOn should not be null.");
            });
        }

        [Test]
        [TestCase("304340a1-16d7-4bdd-b85c-c1521ac3aa2c", "My body does not matter because it will be updated.", "I told you ;)")]
        public async Task UpdateAsyncShouldUpdateEntity(string id, string brandName, string modifiedBody)
        {
            // Arrange
            var repository = new EfRepository<Brand, Guid>(this.dbContext);
            var guid = Guid.Parse(id);
            await this.dbContext.AddAsync(new Brand() { Id = guid, BrandName = brandName });
            await this.dbContext.SaveChangesAsync();

            // Act
            var entity = await this.dbContext.Brands.FindAsync(guid)
                ?? throw new InvalidOperationException(string.Format(MessageConstants.Errors.NoEntityWithPropertyFound, "entity", nameof(id)));
            entity.BrandName = modifiedBody;
            await repository.UpdateAsync(guid);

            // Assert
            Assert.That(this.dbContext.Entry(entity).State, Is.EqualTo(EntityState.Modified), "Entity's state should be Modified.");
            await repository.SaveChangesAsync();

            entity = await this.dbContext.Brands.FindAsync(guid)
                ?? throw new InvalidOperationException(string.Format(MessageConstants.Errors.EntityNotFound, "entity"));
            Assert.Multiple(() =>
            {
                Assert.That(entity, Is.Not.Null, "Entity cannot be found.");
                Assert.That(entity.BrandName, Is.EqualTo(modifiedBody), "Entity's data is not modified.");
                Assert.That(entity.ModifiedOn, Is.Not.Null, "ModifiedOn should not be null.");
            });
        }

        [Test]
        [TestCase("efcd5ace-febb-4999-9530-6726608e6768", "My body does not matter because it will be updated.", "I told you ;)")]
        public async Task UpdateAsyncShouldTrackAndUpdateEntity(string id, string brandName, string modifiedBody)
        {
            // Arrange
            var repository = new EfRepository<Brand, Guid>(this.dbContext);
            var guid = Guid.Parse(id);
            await this.dbContext.AddAsync(new Brand() { Id = guid, BrandName = brandName });
            await this.dbContext.SaveChangesAsync();

            // Act
            var entity = await this.dbContext.Brands.FindAsync(guid)
                ?? throw new InvalidOperationException(string.Format(MessageConstants.Errors.EntityNotFound, "entity"));
            this.dbContext.Entry(entity).State = EntityState.Detached;

            entity.BrandName = modifiedBody;
            await repository.UpdateAsync(guid);

            // Assert
            entity = await this.dbContext.Brands.FindAsync(guid)
                ?? throw new InvalidOperationException(string.Format(MessageConstants.Errors.EntityNotFound, "entity"));

            Assert.Multiple(() =>
            {
                Assert.That(this.dbContext.Entry(entity).State, Is.Not.EqualTo(EntityState.Detached), "Entity should be tracked.");
                Assert.That(this.dbContext.Entry(entity).State, Is.EqualTo(EntityState.Modified), "Entity's state should be Modified.");
            });

            entity.BrandName = modifiedBody;
            await repository.SaveChangesAsync();

            entity = await this.dbContext.Brands.FindAsync(guid)
                ?? throw new InvalidOperationException(string.Format(MessageConstants.Errors.EntityNotFound, "entity"));
            Assert.Multiple(() =>
            {
                Assert.That(entity, Is.Not.Null, "Entity cannot be found.");
                Assert.That(entity.BrandName, Is.EqualTo(modifiedBody), "Entity's data is not modified.");
                Assert.That(entity.ModifiedOn, Is.Not.Null, "ModifiedOn should not be null.");
            });
        }

        [Test]
        [TestCase("1626c009-2a86-4516-818f-44dc4ae17b0a")]
        public void UpdateAsyncThrowsWhenEntityDoesNotExist(string id)
        {
            // Arrange
            var repository = new EfRepository<Brand, Guid>(this.dbContext);

            // Act
            async Task Code() => await repository.UpdateAsync(Guid.Parse(id));

            // Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await Code(), "Invalid operation exception should be thrown.");
        }

        [Test]
        [TestCase("I will be updated!", "I think I'm going to be updated too!", "Yes")]
        public async Task UpdateRangeShouldUpdateAllEntities(string brandName1, string brandName2, string addition)
        {
            // Arrange
            var repository = new EfRepository<Brand, Guid>(this.dbContext);
            var brandList = new List<Brand>()
            {
                new () { BrandName = brandName1 },
                new () { BrandName = brandName2 },
            };
            await repository.AddRangeAsync(brandList);
            await repository.SaveChangesAsync();

            // Act
            var entity1 = await this.dbContext
                .Brands
                .FirstAsync(rn => rn.BrandName == brandName1);
            entity1.BrandName += addition;
            var entity2 = await this.dbContext
                .Brands
                .FirstAsync(rn => rn.BrandName == brandName2);
            entity2.BrandName += addition;

            repository.UpdateRange(new List<Brand>() { entity1, entity2 });
            await repository.SaveChangesAsync();

            // Assert
            entity1 = await this.dbContext
                .Brands
                .FirstAsync(rn => rn.BrandName == brandName1 + addition);
            Assert.Multiple(() =>
            {
                Assert.That(entity1, Is.Not.Null, "The entity cannot be found.");
                Assert.That(entity1.ModifiedOn, Is.Not.Null, "ModifiedOn should not be null.");
                Assert.That(entity1.BrandName, Is.EqualTo(brandName1 + addition), "Entity data is not set or is not matching.");
            });
            entity2 = await this.dbContext
                .Brands
                .FirstAsync(rn => rn.BrandName == brandName2 + addition);
            Assert.Multiple(() =>
            {
                Assert.That(entity2, Is.Not.Null, "The entity cannot be found.");
                Assert.That(entity2.ModifiedOn, Is.Not.Null, "ModifiedOn should not be null.");
                Assert.That(entity2.BrandName, Is.EqualTo(brandName2 + addition), "Entity data is not set or is not matching.");
            });
        }

        [Test]
        [TestCase("d429e67c-d9d7-4a93-9ba7-e35ad2150182", "I'm going to be deleted :(")]
        public async Task DeleteShouldChangeEntityStateAndDeleteIt(string id, string brandName)
        {
            // Arrange
            var repository = new EfRepository<Brand, Guid>(this.dbContext);
            var guid = Guid.Parse(id);
            await this.dbContext.AddAsync(new Brand() { Id = guid, BrandName = brandName });
            await this.dbContext.SaveChangesAsync();

            // Act
            var entity = await this.dbContext.Brands.FindAsync(guid)
                ?? throw new InvalidOperationException(string.Format(MessageConstants.Errors.EntityNotFound, "entity"));
            repository.Delete(entity);

            // Assert
            Assert.That(this.dbContext.Entry(entity).State, Is.EqualTo(EntityState.Deleted), "Entity's state should be Deleted.");
            await repository.SaveChangesAsync();

            entity = await this.dbContext.Brands.FirstOrDefaultAsync(rn => rn.Id == guid);
            Assert.That(entity, Is.Null, "Entity is not deleted.");
        }

        [Test]
        [TestCase("3edef0b0-912d-497a-ae8c-d183b898d627", "I'm going to be deleted :(")]
        public async Task DeleteAsyncShouldChangeEntityStateAndDeleteIt(string id, string brandName)
        {
            // Arrange
            var repository = new EfRepository<Brand, Guid>(this.dbContext);
            var guid = Guid.Parse(id);
            await this.dbContext.AddAsync(new Brand() { Id = guid, BrandName = brandName });
            await this.dbContext.SaveChangesAsync();

            // Act
            var entity = await this.dbContext.Brands.FindAsync(guid)
                ?? throw new InvalidOperationException(string.Format(MessageConstants.Errors.EntityNotFound, "entity"));
            await repository.DeleteAsync(guid);

            // Assert
            Assert.That(this.dbContext.Entry(entity).State, Is.EqualTo(EntityState.Deleted), "Entity's state should be Deleted.");
            await repository.SaveChangesAsync();

            entity = await this.dbContext.Brands.FirstOrDefaultAsync(rn => rn.Id == guid);
            Assert.That(entity, Is.Null, "Entity is not deleted.");
        }

        [Test]
        [TestCase("0afc3501-cdfe-4d2e-91a4-15a41ac0d5db")]
        public void DeleteAsyncShouldThrowWhenEntityDoesNotExist(string id)
        {
            // Arrange
            var repository = new EfRepository<Brand, Guid>(this.dbContext);

            // Act
            async Task Code() => await repository.DeleteAsync(Guid.Parse(id));

            // Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await Code(), "Invalid operation exception should be thrown.");
        }

        [Test]
        [TestCase("We are going to be deleted!?", "I don't like where this is going")]
        public async Task DeleteRangeShouldRemoveEntities(string brandName1, string brandName2)
        {
            // Arrange
            var repository = new EfRepository<Brand, Guid>(this.dbContext);
            var brandList = new List<Brand>()
            {
                new () { BrandName = brandName1 },
                new () { BrandName = brandName2 },
            };
            await repository.AddRangeAsync(brandList);
            await repository.SaveChangesAsync();

            // Act
            var entity1 = await this.dbContext
                .Brands
                .FirstAsync(rn => rn.BrandName == brandName1);
            var entity2 = await this.dbContext
                .Brands
                .FirstAsync(rn => rn.BrandName == brandName2);

            repository.DeleteRange(new List<Brand>() { entity1, entity2 });

            // Assert
            entity1 = await this.dbContext
                .Brands
                .FirstAsync(rn => rn.BrandName == brandName1);
            entity2 = await this.dbContext
                .Brands
                .FirstAsync(rn => rn.BrandName == brandName2);
            Assert.Multiple(() =>
            {
                Assert.That(this.dbContext.Entry(entity1).State, Is.EqualTo(EntityState.Deleted), "Entity's state should be Deleted.");
                Assert.That(this.dbContext.Entry(entity2).State, Is.EqualTo(EntityState.Deleted), "Entity's state should be Deleted.");
            });
            await repository.SaveChangesAsync();

            entity1 = await this.dbContext
                .Brands
                .FirstOrDefaultAsync(rn => rn.BrandName == brandName1);
            Assert.That(entity1, Is.Null, "The entity is not deleted.");

            entity2 = await this.dbContext
                .Brands
                .FirstOrDefaultAsync(rn => rn.BrandName == brandName2);
            Assert.That(entity2, Is.Null, "The entity is not deleted.");
        }

        [Test]
        [TestCase("9a14c00e-dbd6-4b05-9541-2af3f5ec6f31", "Am I detached?")]
        public async Task DetachShouldStopTrackingEntity(string id, string brandName)
        {
            // Arrange
            var repository = new EfRepository<Brand, Guid>(this.dbContext);
            var guid = Guid.Parse(id);
            await this.dbContext.AddAsync(new Brand() { Id = guid, BrandName = brandName });
            await this.dbContext.SaveChangesAsync();

            // Act
            var entity = await this.dbContext.Brands.FindAsync(guid)
                ?? throw new InvalidOperationException(string.Format(MessageConstants.Errors.EntityNotFound, "entity"));
            this.dbContext.Entry(entity).State = EntityState.Modified;
            repository.Detach(entity);

            // Assert
            Assert.That(this.dbContext.Entry(entity).State, Is.Not.EqualTo(EntityState.Modified), "Entity's state should be changed.");
            Assert.That(this.dbContext.Entry(entity).State, Is.EqualTo(EntityState.Detached), "Entity's state should be Deleted.");
        }
    }
}
