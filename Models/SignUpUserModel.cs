namespace TicketStore.Models
{
    public class SignUpUserModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PhonePrefix { get; set; }
        public int Age { get; set; }
        public bool IsStudent { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}