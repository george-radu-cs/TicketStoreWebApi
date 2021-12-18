using System;

namespace TicketStore.Entities
{
    public class Ticket
    {
        public string UserId { get; set; }
        public string EventId { get; set; }
        public string TicketType { get; set; }
        public string Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        public virtual User User { get; set; }
        public virtual Event Event { get; set; }
    }
}