﻿namespace AwesomeBackend.Shared.Models.Responses
{
    public class Address
    {
        public string PostalCode { get; set; }

        public string Street { get; set; }

        public string Location { get; set; }

        public string Province { get; set; }

        public override string ToString()
        {
            var address = $"{Street}, {PostalCode}, {Location} ({Province})";
            return address;
        }
    }
}
