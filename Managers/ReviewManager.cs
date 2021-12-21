using System;
using System.Collections.Generic;
using System.Linq;
using TicketStore.Entities;
using TicketStore.Models;
using TicketStore.Repositories;
using TicketStore.ResponseModels;
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

        public Review GetReviewById(string id)
        {
            var review = _reviewRepository.GetReviewsWithUserAndEventIQueryable()
                .FirstOrDefault(r => r.Id == id);

            return review;
        }
        
        public ReviewResponseModel GetReviewResponseById(string id)
        {
            var review = _reviewRepository.GetReviewsWithUserAndEventIQueryable()
                .FirstOrDefault(r => r.Id == id);

            return ConvertToReviewResponseModelWithUserAndEvent(review);
        }

        public List<ReviewResponseModel> GetReviewsResponse()
        {
            var reviews = _reviewRepository.GetReviewsIQueryable()
                .Select(r => ConvertToReviewResponseModel(r))
                .ToList();

            return reviews;
        }

        public List<ReviewResponseModel> GetUserReviewsResponse(string userId)
        {
            var reviews = _reviewRepository.GetReviewsWithEventIQueryable()
                .Where(r => r.UserId == userId)
                .Select(r => ConvertToReviewResponseModelWithEvent(r))
                .ToList();

            return reviews;
        }

        public List<ReviewResponseModel> GetEventReviewsResponse(string eventId)
        {
            var reviews = _reviewRepository.GetReviewsWithUserIQueryable()
                .Where(r => r.EventId == eventId)
                .Select(r => ConvertToReviewResponseModelWithUser(r))
                .ToList();

            return reviews;
        }

        public void Create(ReviewModel model)
        {
            var date = DateTime.Now.ToUniversalTime();
            var newReview = new Review
            {
                Title = model.Title,
                Message = model.Message,
                Rating =model.Rating,
                CreatedAt = date,
                UpdatedAt = date,
                UserId = model.UserId,
                EventId = model.EventId,
            };
            
            _reviewRepository.Create(newReview);
        }

        public void Update(ReviewModel model)
        {
            var updatedReview = GetReviewById(model.Id);
            var date = DateTime.Now.ToUniversalTime();
            updatedReview.Title = model.Title;
            updatedReview.Message = model.Message;
            updatedReview.Rating = model.Rating;
            updatedReview.UpdatedAt=date;
            
            _reviewRepository.Update(updatedReview);
        }

        public void Delete(string id)
        {
            var review = GetReviewById(id);

            _reviewRepository.Delete(review);
        }
    }
}