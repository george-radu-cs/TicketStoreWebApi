using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace TicketStore.Entities
{
    public class Role : IdentityRole
    {
        public ICollection<UserRole> UserRoles { get; set; }
    }
}