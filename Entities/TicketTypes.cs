using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketStore.Entities
{
    public class TicketTypes
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        // the child & student tickets will offer the same place at the event as a standard ticket but at a lower price
        // standard + child + student = total normal tickets available
        public int NumberStandardTickets { get; set; }
        public string PriceStandardTicket { get; set; }
        public int NumberVipTickets { get; set; }
        public string PriceVipTicket { get; set; }
        public string PriceChildTicket { get; set; }
        public string PriceStudentTicket { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}