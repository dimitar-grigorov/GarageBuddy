namespace GarageBuddy.Common.Core.Wrapper.Generic
{
    using System.Collections.Generic;

    public class PaginatedResult<T> : Result
    {
        public PaginatedResult(List<T> data)
        {
            Data = data;
        }

        internal PaginatedResult(bool succeeded, List<T> data, List<string> messages, int totalCount)
        {
            Data = data;
            Messages = messages;
            Succeeded = succeeded;
            TotalCount = totalCount;
        }

        public int TotalCount { get; set; }

        public List<T> Data { get; set; }

        public static PaginatedResult<T> Failure(List<string> messages)
        {
            return new PaginatedResult<T>(false, new List<T>(), messages, 0);
        }

        public static PaginatedResult<T> Success(List<T> data, int totalCount)
        {
            return new PaginatedResult<T>(true, data, new List<string>(), totalCount);
        }
    }
}
