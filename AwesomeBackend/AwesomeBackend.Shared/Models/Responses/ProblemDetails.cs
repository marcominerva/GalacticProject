using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AwesomeBackend.Shared.Models.Responses
{
    public class ProblemDetails
    {
        public string Type { get; init; }

        public string Title { get; init; }

        public int? Status { get; init; }

        public string Detail { get; init; }

        public string Instance { get; init; }

        [JsonExtensionData]
        public IDictionary<string, object> Extensions { get; init; }

        public Dictionary<string, string[]> Errors { get; init; }
    }
}
