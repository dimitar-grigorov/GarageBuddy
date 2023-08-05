namespace GarageBuddy.Common.Core.Wrapper
{
    using System.Collections.Generic;

    public interface IResult
    {
        List<string> Messages { get; set; }

        bool Succeeded { get; set; }
    }
}
