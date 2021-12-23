using System;

namespace TicketStore.Models
{
    public class EventModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Category { get; set; }
        public string Genre { get; set; }
        public string OrganizerId { get; set; }
        public LocationModel Location { get; set; }
        public TicketTypesModels TicketTypes { get; set; }
    }
}