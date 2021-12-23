namespace TicketStore.ResponseModels
{
    public class TicketResponseModel
    {
        public string UserId { get; set; }
        public string EventId { get; set; }
        public string AuxiliaryId { get; set; }
        public string TicketType { get; set; }
        public string Price { get; set; }
        public UserResponseModel Buyer { get; set; }
        public EventResponseModel Event { get; set; }
    }
}