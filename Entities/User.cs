using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace TicketStore.Entities
{
    public class User : IdentityUser
    {
        public ICollection<UserRole> UserRoles { get; set; }
    }
}