using TicketStore.Entities;
using TicketStore.Models;
using TicketStore.ResponseModels;

namespace TicketStore.Managers
{
    public interface IReviewManager
    {
        Review GetReviewById(string userId, string eventId);
        ResponseRecordWithErrors<ReviewResponseModel> GetReviewResponseById(string userId, string eventId);
        ResponseRecordsListWithErrors<ReviewResponseModel> GetReviewsResponse();
        ResponseRecordsListWithErrors<ReviewResponseModel> GetUserReviewsResponse(string userId);
        ResponseRecordsListWithErrors<ReviewResponseModel> GetOrganizerReviewsResponse(string organizerId);
        ResponseRecordsListWithErrors<ReviewResponseModel> GetEventReviewsResponse(string eventId);
        ResponseSuccessWithErrors Create(ReviewModel model);
        ResponseSuccessWithErrors Update(ReviewModel model);
        ResponseSuccessWithErrors Delete(string userId, string eventId);
    }
}