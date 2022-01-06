using System.Collections.Generic;
using TicketStore.Entities;
using TicketStore.Models;
using TicketStore.ResponseModels;

namespace TicketStore.Managers
{
    public interface IReviewManager
    {
        Review GetReviewById(string userId, string eventId);

        (ReviewResponseModel resReview, string errorMessage, string errorType) GetReviewResponseById(string userId,
            string eventId);

        (List<ReviewResponseModel> resReviews, string errorMessage, string errorType) GetReviewsResponse();

        (List<ReviewResponseModel> resReviews, string errorMessage, string errorType) GetUserReviewsResponse(
            string userId);

        (List<ReviewResponseModel> resReviews, string errorMessage, string errorType) GetOrganizerReviewsResponse(
            string organizerId);

        (List<ReviewResponseModel> resReviews, string errorMessage, string errorType) GetEventReviewsResponse(
            string eventId);

        (bool success, string errorMessage, string errorType) Create(ReviewModel model);
        (bool success, string errorMessage, string errorType) Update(ReviewModel model);
        (bool success, string errorMessage, string errorType) Delete(string userId, string eventId);
    }
}