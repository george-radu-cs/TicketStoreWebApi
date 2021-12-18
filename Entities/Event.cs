using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketStore.Entities
{
    public sealed class Event
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Concert, Sports, Arts, Theater
        public string Category { get; set; }

        // Pop, Electronic, Football, 
        public string Genre { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public string TicketTypesId { get;set; }
        public TicketTypes TicketTypes { get; set; }
        public string LocationId { get; set; }
        public Location Location { get; set; }
        public string OrganizerId { get; set; }
        public User Organizer { get; set; }
    }
}