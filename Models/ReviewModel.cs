namespace TicketStore.Models
{
    public class ReviewModel
    {
        public string UserId { get; set; }
        public string EventId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public int Rating { get; set; }
    }
}