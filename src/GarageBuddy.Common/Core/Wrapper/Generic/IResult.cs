namespace GarageBuddy.Common.Core.Wrapper.Generic
{
    public interface IResult<out T> : IResult
    {
        T Data { get; }
    }
}
