using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketStore.Entities
{
    public class Guest
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SceneName { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Genre { get; set; }
        public int Age { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string EventId { get; set; }
        public virtual Event Event { get; set; }
    }
}