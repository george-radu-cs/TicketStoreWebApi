namespace TicketStore.ResponseModels
{
    public class GuestResponseModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SceneName { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Genre { get; set; }
        public int Age { get; set; }
        public string EventId { get;set; }
    }
}