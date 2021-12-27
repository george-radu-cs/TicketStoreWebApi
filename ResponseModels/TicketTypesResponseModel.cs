using System;

namespace TicketStore.ResponseModels
{
    public class TicketTypesResponseModel
    {
        public string Id { get; set; }
        public int NumberStandardTickets { get; set; }
        public string PriceStandardTicket { get; set; }
        public int NumberVipTickets { get; set; }
        public string PriceVipTicket { get; set; }
        public string PriceChildTicket { get; set; }
        public string PriceStudentTicket { get; set; }
    }
}