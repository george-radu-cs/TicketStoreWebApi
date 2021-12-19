namespace TicketStore.Models
{
    public class ReviewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Message{get; set; }
        public string Rating { get; set; }
        public string UserId { get; set; }
        public string EventId{get; set; }
    }
}