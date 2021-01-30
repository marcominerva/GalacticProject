using AwesomeBackend.Shared.Models.Responses;
using System.Collections.Generic;
using System.Text.Json;

namespace AwesomeBackend.Shared.Extensions
{
    public static class ProblemDetailsExtensions
    {
        private static readonly JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);

        public static Dictionary<string, string[]> GetErrors(this ProblemDetails problemDetails)
        {
            if (problemDetails.Extensions?.ContainsKey("errors") ?? false)
            {
                var errors = JsonSerializer.Deserialize<Dictionary<string, string[]>>(problemDetails.Extensions["errors"].ToString(), jsonSerializerOptions);
                return errors;
            }

            return new Dictionary<string, string[]>();
        }
    }
}
