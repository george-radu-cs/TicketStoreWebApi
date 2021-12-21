namespace TicketStore.ResponseModels
{
    public class ReviewResponseModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Rating { get; set; }
        public UserResponseModel User { get; set; }
        public EventResponseModel Event { get; set; }
    }
}