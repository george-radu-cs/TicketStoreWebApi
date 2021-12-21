using System;
using System.Collections.Generic;

namespace TicketStore.ResponseModels
{
    public class UserResponseModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string PhonePrefix { get; set; }
        public int Age { get; set; }
        public bool IsStudent { get; set; }
        public DateTime JoinDate { get; set; }
        public ICollection<EventResponseModel> EventsCreated { get; set; }
        public ICollection<TicketResponseModel> TicketsBought { get; set; }
        public ICollection<ReviewResponseModel> Reviews { get; set; }
    }
}