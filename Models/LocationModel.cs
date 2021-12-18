namespace TicketStore.Models
{
    public class LocationModel
    {
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
    }
}