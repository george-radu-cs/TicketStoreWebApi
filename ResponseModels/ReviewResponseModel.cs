using System;

namespace TicketStore.ResponseModels
{
    public class ReviewResponseModel
    {
        public string UserId { get; set; }
        public string EventId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public int Rating { get; set; }
        public DateTime Date { get; set; }
        public UserResponseModel User { get; set; }
        public EventResponseModel Event { get; set; }
    }
}