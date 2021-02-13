using System;

namespace AwesomeBackend.Shared.Models.Requests
{
    public class RatingRequest
    {
        public int Score { get; set; }

        public string Comment { get; set; }

        public DateTime VisitedAt { get; set; } = DateTime.Now;
    }
}
