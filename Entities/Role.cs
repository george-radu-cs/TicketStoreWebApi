using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace TicketStore.Entities
{
    public class Role : IdentityRole
    {
        public ICollection<UserRole> UserRoles { get; set; }
    }

    public static class AuthorizationRoles
    {
        public const string Admin = "Admin";
        public const string Buyer = "Buyer";
        public const string Organizer = "Organizer";

        public const string BuyerOrAdmin = Admin + "," + Buyer;
        public const string OrganizerOrAdmin = Admin + "," + Organizer;
        public const string BuyerOrOrganizer = Buyer + "," + Organizer;
        public const string Anyone = Admin + "," + Buyer + "," + Organizer;
    }
}