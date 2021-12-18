using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketStore.Entities
{
    public sealed class Location
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string BuildingName { get; set; }
        public string AddressFullName { get; set; }
        public string Locality { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string PostalCode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string GeocodeAccuracy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}