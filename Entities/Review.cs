using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketStore.Entities
{
    public class Review
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }
        public string EventId { get; set; }
        public virtual Event Event { get; set; }
    }
}