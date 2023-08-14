namespace GarageBuddy.Services.Mapping.TypeConverters
{
    using System;

    using AutoMapper;

    public class DateTimeTypeConverter : ITypeConverter<string, DateTime>
    {
        public DateTime Convert(string source, DateTime destination, ResolutionContext context)
        {
            return System.Convert.ToDateTime(source);
        }
    }
}
