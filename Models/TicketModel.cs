namespace TicketStore.Models
{
    public class TicketModel
    {
        public string UserId { get; set; }
        public string EventId { get; set; }
        public string AuxiliaryId { get; set; }
        public string TicketType { get; set; }
        public string Price { get; set; }
        public string PriceCurrency { get; set; }
    }
}