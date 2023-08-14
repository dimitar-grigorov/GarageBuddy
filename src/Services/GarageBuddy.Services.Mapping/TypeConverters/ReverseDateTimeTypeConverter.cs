namespace GarageBuddy.Services.Mapping.TypeConverters
{
    using System;

    using AutoMapper;

    using Common.Constants;

    public class ReverseDateTimeTypeConverter : ITypeConverter<DateTime?, string>
    {
        public string Convert(DateTime? source, string destination, ResolutionContext context)
        {
            return source?.ToString(GlobalConstants.DefaultDateTimeFormat);
        }
    }
}
