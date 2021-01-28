using System.Collections.Generic;

namespace AwesomeBackend.Shared.Models.Responses
{
    public class ProblemDetails
    {
        public string Type { get; set; }

        public string Title { get; set; }

        public int? Status { get; set; }

        public string Detail { get; set; }

        public string Instance { get; set; }

        public IDictionary<string, object> Extensions { get; }
    }
}
