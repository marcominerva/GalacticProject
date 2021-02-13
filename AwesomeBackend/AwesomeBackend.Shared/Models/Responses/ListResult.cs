using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace AwesomeBackend.Shared.Models.Responses
{
    public class ListResult<T>
    {
        public IEnumerable<T> Content { get; init; }

        public bool HasNextPage { get; init; }

        public long TotalCount { get; init; }

        public ListResult(IEnumerable<T> content, bool hasNextPage = false)
        {
            Content = content;
            TotalCount = content?.LongCount() ?? 0;
            HasNextPage = hasNextPage;
        }

        [JsonConstructor]
        public ListResult(IEnumerable<T> content, long totalCount, bool hasNextPage = false)
        {
            Content = content;
            TotalCount = totalCount;
            HasNextPage = hasNextPage;
        }
    }
}
