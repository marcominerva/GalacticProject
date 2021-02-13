using System;

namespace AwesomeBackend.Shared.Models.Responses
{
    public class Rating
    {
        public Guid Id { get; set; }

        public double Score { get; set; }

        public string Comment { get; set; }

        public DateTime Date { get; set; }
    }
}
