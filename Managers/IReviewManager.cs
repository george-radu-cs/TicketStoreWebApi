using System.Collections.Generic;
using TicketStore.Entities;
using TicketStore.Models;

namespace TicketStore.Managers
{
    public interface IReviewManager
    {
        List<Review> GetReviews();
        List<Review> GetUserReviews(string userId);
        List<Review> GetEventReviews(string eventId);
        Review GetReviewById(string id);
        void Create(ReviewModel model);
        void Update(ReviewModel model);
        void Delete(string id);
    }
}