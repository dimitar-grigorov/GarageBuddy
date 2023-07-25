namespace GarageBuddy.Data
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using GarageBuddy.Data.Common.Models;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata;

    internal static class GlobalEntityConfiguration
    {
        public static void AddIndexForDeletableEntities(ModelBuilder modelBuilder, IEnumerable<IMutableEntityType> deletableEntityTypes)
        {
            // IDeletableEntity.IsDeleted index
            foreach (var deletableEntityType in deletableEntityTypes)
            {
                modelBuilder.Entity(deletableEntityType.ClrType).HasIndex(nameof(IDeletableEntity.IsDeleted));
            }
        }

        public static void DisableCascadeDelete(ModelBuilder modelBuilder, IEnumerable<IMutableEntityType> entityTypes)
        {
            var foreignKeys = entityTypes
                .SelectMany(e => e.GetForeignKeys().Where(f => f.DeleteBehavior == DeleteBehavior.Cascade));
            foreach (var foreignKey in foreignKeys)
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}
