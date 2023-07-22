namespace GarageBuddy.Data.Extensions;

using System;
using System.Linq;

using Microsoft.EntityFrameworkCore;

// https://stackoverflow.com/questions/34557574/how-to-create-a-table-corresponding-to-enum-in-ef6-code-first
public static class DbSetExtensions
{
    public static void SeedEnumValues<T, TEnum>(this DbSet<T> dbSet, Func<TEnum, T> converter)
        where T : class
    {
        Enum.GetValues(typeof(TEnum))
                .Cast<object>()
                .Select(value => converter((TEnum)value))
                .ToList()
                .ForEach(instance => dbSet.Add(instance));
    }
}
