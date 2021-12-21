using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace TicketStore.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public override string PhoneNumber { get; set; }
        public string PhonePrefix { get; set; }
        public int Age { get; set; }
        public bool IsStudent { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }
        // empty if user is not organizer
        public virtual ICollection<Event> EventsCreated { get; set; }
        // empty if user is not buyer
        public virtual ICollection<Ticket> Tickets { get; set; }
        // empty if user is not buyer
        public virtual ICollection<Review> Reviews { get;set; }
    }
}