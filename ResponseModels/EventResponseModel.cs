using System;

namespace TicketStore.ResponseModels
{
    public class EventResponseModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Category { get; set; }
        public string Genre { get; set; }
        public LocationResponseModel Location { get; set; }
        public TicketTypesResponseModel TicketTypes { get; set; }
        public UserResponseModel Organizer { get; set; }
    }
}