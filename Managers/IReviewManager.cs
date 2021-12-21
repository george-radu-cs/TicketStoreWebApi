using System.Collections.Generic;
using TicketStore.Entities;
using TicketStore.Models;
using TicketStore.ResponseModels;

namespace TicketStore.Managers
{
    public interface IReviewManager
    {
        Review GetReviewById(string id);
        ReviewResponseModel GetReviewResponseById(string id);
        List<ReviewResponseModel> GetReviewsResponse();
        List<ReviewResponseModel> GetUserReviewsResponse(string userId);
        List<ReviewResponseModel> GetEventReviewsResponse(string eventId);
        void Create(ReviewModel model);
        void Update(ReviewModel model);
        void Delete(string id);
    }
}