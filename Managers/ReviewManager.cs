using System;
using System.Collections.Generic;
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

        public (ReviewResponseModel resReview, string errorMessage, string errorType) GetReviewResponseById(
            string userId, string eventId)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(eventId))
            {
                return (resReview: null, errorMessage: "The Review's Id is required", errorType: ErrorTypes.UserFault);
            }

            var review = GetReviewById(userId, eventId);
            if (review == null)
            {
                return (resReview: null, errorMessage: "Review not found", ErrorTypes.NotFound);
            }

            return (resReview: ConvertToReviewResponseModelWithUserAndEvent(review), errorMessage: null,
                errorType: null);
        }

        public (List<ReviewResponseModel> resReviews, string errorMessage, string errorType) GetReviewsResponse()
        {
            var reviews = _reviewRepository.GetReviewsIQueryable()
                .Select(r => ConvertToReviewResponseModel(r))
                .ToList();

            if (reviews.IsNullOrEmpty())
            {
                return (resReviews: null, errorMessage: "Reviews not found", errorType: ErrorTypes.NotFound);
            }

            return (resReviews: reviews, errorMessage: null, errorType: null);
        }

        public (List<ReviewResponseModel> resReviews, string errorMessage, string errorType) GetUserReviewsResponse(
            string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return (resReviews: null, errorMessage: "The UserId is required", errorType: ErrorTypes.UserFault);
            }

            var reviews = _reviewRepository.GetReviewsWithEventIQueryable()
                .Where(r => r.UserId == userId)
                .Select(r => ConvertToReviewResponseModelWithEvent(r))
                .ToList();

            if (reviews.IsNullOrEmpty())
            {
                return (resReviews: null, errorMessage: "Reviews not found", errorType: ErrorTypes.NotFound);
            }

            return (resReviews: reviews, errorMessage: null, errorType: null);
        }

        public (List<ReviewResponseModel> resReviews, string errorMessage, string errorType) GetEventReviewsResponse(
            string eventId)
        {
            if (string.IsNullOrEmpty(eventId))
            {
                return (resReviews: null, errorMessage: "The EventId is required", errorType: ErrorTypes.UserFault);
            }

            var reviews = _reviewRepository.GetReviewsWithUserIQueryable()
                .Where(r => r.EventId == eventId)
                .Select(r => ConvertToReviewResponseModelWithUser(r))
                .ToList();

            if (reviews.IsNullOrEmpty())
            {
                return (resReviews: null, errorMessage: "Reviews not found", errorType: ErrorTypes.NotFound);
            }

            return (resReviews: reviews, errorMessage: null, errorType: null);
        }

        public (bool success, string errorMessage, string errorType) Create(ReviewModel model)
        {
            var (isValid, validationErrorMessage) = Validations.ValidateReview(model);
            if (!isValid)
            {
                return (success: false, errorMessage: validationErrorMessage, errorType: ErrorTypes.UserFault);
            }

            var date = DateTime.Now.ToUniversalTime();
            var newReview = new Review
            {
                Title = model.Title,
                Message = model.Message,
                Rating = model.Rating,
                CreatedAt = date,
                UpdatedAt = date,
                UserId = model.UserId,
                EventId = model.EventId,
            };

            _reviewRepository.Create(newReview);
            return (success: true, errorMessage: null, errorType: null);
        }

        public (bool success, string errorMessage, string errorType) Update(ReviewModel model)
        {
            var (isValid, validationErrorMessage) = Validations.ValidateReview(model, true);
            if (!isValid)
            {
                return (success: false, errorMessage: validationErrorMessage, errorType: ErrorTypes.UserFault);
            }

            var reviewToUpdate = GetReviewById(model.UserId, model.EventId);
            if (reviewToUpdate == null)
            {
                return (success: false, errorMessage: "The Review doesn't exists", errorType: ErrorTypes.UserFault);
            }

            var date = DateTime.Now.ToUniversalTime();
            reviewToUpdate.Title = model.Title;
            reviewToUpdate.Message = model.Message;
            reviewToUpdate.Rating = model.Rating;
            reviewToUpdate.UpdatedAt = date;

            _reviewRepository.Update(reviewToUpdate);
            return (success: true, errorMessage: null, errorType: null);
        }

        public (bool success, string errorMessage, string errorType) Delete(string userId, string eventId)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(eventId))
            {
                return (success: false, errorMessage: "Review Id is required", errorType: ErrorTypes.UserFault);
            }

            var reviewToDelete = GetReviewById(userId, eventId);
            if (reviewToDelete == null)
            {
                return (success: false, errorMessage: "Review doesn't exists", errorType: ErrorTypes.UserFault);
            }

            _reviewRepository.Delete(reviewToDelete);
            return (success: true, errorMessage: null, errorType: null);
        }
    }
}