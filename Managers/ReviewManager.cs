using System.Linq;
using Castle.Core.Internal;
using TicketStore.Entities;
using TicketStore.Models;
using TicketStore.Repositories;
using TicketStore.ResponseModels;
using TicketStore.Utils;
using static TicketStore.Utils.ResponseConversions;

namespace TicketStore.Managers
{
    public class ReviewManager : IReviewManager
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewManager(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public Review GetReviewById(string userId, string eventId)
        {
            var review = _reviewRepository.GetReviewsWithUserAndEventIQueryable()
                .FirstOrDefault(r => r.UserId == userId && r.EventId == eventId);

            return review;
        }

        public ResponseRecordWithErrors<ReviewResponseModel> GetReviewResponseById(
            string userId, string eventId)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(eventId))
            {
                return new ResponseRecordWithErrors<ReviewResponseModel>(null, "The Review's Id is required",
                    ErrorTypes.UserFault);
            }

            var review = GetReviewById(userId, eventId);
            if (review == null)
            {
                return new ResponseRecordWithErrors<ReviewResponseModel>(null, "Review not found", ErrorTypes.NotFound);
            }

            return new ResponseRecordWithErrors<ReviewResponseModel>(
                ConvertToReviewResponseModelWithUserAndEvent(review), null, null);
        }

        public ResponseRecordsListWithErrors<ReviewResponseModel> GetReviewsResponse()
        {
            var reviews = _reviewRepository.GetReviewsIQueryable()
                .OrderByDescending(r => r.UpdatedAt)
                .Select(r => ConvertToReviewResponseModel(r))
                .ToList();

            if (reviews.IsNullOrEmpty())
            {
                return new ResponseRecordsListWithErrors<ReviewResponseModel>(null, "Reviews not found",
                    ErrorTypes.NotFound);
            }

            return new ResponseRecordsListWithErrors<ReviewResponseModel>(reviews, null, null);
        }

        public ResponseRecordsListWithErrors<ReviewResponseModel> GetUserReviewsResponse(
            string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return new ResponseRecordsListWithErrors<ReviewResponseModel>(null, "The UserId is required",
                    ErrorTypes.UserFault);
            }

            var reviews = _reviewRepository.GetReviewsWithEventIQueryable()
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.UpdatedAt)
                .Select(r => ConvertToReviewResponseModelWithEvent(r))
                .ToList();

            if (reviews.IsNullOrEmpty())
            {
                return new ResponseRecordsListWithErrors<ReviewResponseModel>(null, "Reviews not found",
                    ErrorTypes.NotFound);
            }

            return new ResponseRecordsListWithErrors<ReviewResponseModel>(reviews, null, null);
        }


        public ResponseRecordsListWithErrors<ReviewResponseModel> GetOrganizerReviewsResponse(string organizerId)
        {
            if (string.IsNullOrEmpty(organizerId))
            {
                return new ResponseRecordsListWithErrors<ReviewResponseModel>(null, "The OrganizerId is required",
                    ErrorTypes.UserFault);
            }

            var reviews = _reviewRepository.GetReviewsWithUserAndEventIQueryable()
                .Where(r => r.Event.OrganizerId == organizerId)
                .OrderByDescending(r => r.UpdatedAt)
                .Select(r => ConvertToReviewResponseModelWithUserAndEvent(r))
                .ToList();

            if (reviews.IsNullOrEmpty())
            {
                return new ResponseRecordsListWithErrors<ReviewResponseModel>(null, "Reviews not found",
                    ErrorTypes.NotFound);
            }

            return new ResponseRecordsListWithErrors<ReviewResponseModel>(reviews, null, null);
        }

        public ResponseRecordsListWithErrors<ReviewResponseModel> GetEventReviewsResponse(string eventId)
        {
            if (string.IsNullOrEmpty(eventId))
            {
                return new ResponseRecordsListWithErrors<ReviewResponseModel>(null, "The EventId is required",
                    ErrorTypes.UserFault);
            }

            var reviews = _reviewRepository.GetReviewsWithUserIQueryable()
                .Where(r => r.EventId == eventId)
                .OrderByDescending(r => r.UpdatedAt)
                .Select(r => ConvertToReviewResponseModelWithUser(r))
                .ToList();

            if (reviews.IsNullOrEmpty())
            {
                return new ResponseRecordsListWithErrors<ReviewResponseModel>(null, "Reviews not found",
                    ErrorTypes.NotFound);
            }

            return new ResponseRecordsListWithErrors<ReviewResponseModel>(reviews, null, null);
        }

        public ResponseSuccessWithErrors Create(ReviewModel model)
        {
            var (isValid, validationErrorMessage) = Validations.ValidateReview(model);
            if (!isValid)
            {
                return new ResponseSuccessWithErrors(false, validationErrorMessage, ErrorTypes.UserFault);
            }

            var newReview = EntityConversions.ConvertToReviewEntity(model);
            _reviewRepository.Create(newReview);
            return new ResponseSuccessWithErrors(true, null, null);
        }

        public ResponseSuccessWithErrors Update(ReviewModel model)
        {
            var (isValid, validationErrorMessage) = Validations.ValidateReview(model, true);
            if (!isValid)
            {
                return new ResponseSuccessWithErrors(false, validationErrorMessage, ErrorTypes.UserFault);
            }

            var reviewToUpdate = GetReviewById(model.UserId, model.EventId);
            if (reviewToUpdate == null)
            {
                return new ResponseSuccessWithErrors(false, "The Review doesn't exists", ErrorTypes.UserFault);
            }

            var updatedReview = EntityConversions.ConvertToReviewEntity(model, true, reviewToUpdate);
            _reviewRepository.Update(updatedReview);
            return new ResponseSuccessWithErrors(true, null, null);
        }

        public ResponseSuccessWithErrors Delete(string userId, string eventId)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(eventId))
            {
                return new ResponseSuccessWithErrors(false, "Review Id is required", ErrorTypes.UserFault);
            }

            var reviewToDelete = GetReviewById(userId, eventId);
            if (reviewToDelete == null)
            {
                return new ResponseSuccessWithErrors(false, "Review doesn't exists", ErrorTypes.UserFault);
            }

            _reviewRepository.Delete(reviewToDelete);
            return new ResponseSuccessWithErrors(true, null, null);
        }
    }
}