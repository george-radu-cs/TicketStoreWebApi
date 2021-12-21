using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketStore.Entities;
using TicketStore.Managers;
using TicketStore.Models;

namespace TicketStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewManager _reviewManager;

        public ReviewController(IReviewManager reviewManager)
        {
            _reviewManager = reviewManager;
        }

        [HttpGet("reviews")]
        [Authorize(Policy = AuthorizationRoles.Admin)]
        public async Task<IActionResult> GetReviews()
        {
            try
            {
                var reviews = _reviewManager.GetReviewsResponse();

                return Ok(reviews);
            }
            catch (Exception e)
            {
                return BadRequest("couldn't get the reviews");
            }
        }

        [HttpGet("byId/{id}")]
        [Authorize(Policy = AuthorizationRoles.Anyone)]
        public async Task<IActionResult> GetReview([FromRoute] string id)
        {
            try
            {
                var review = _reviewManager.GetReviewResponseById(id);

                return Ok(review);
            } catch (Exception)
            {
                return BadRequest("couldn't get the review");
            }
        }

        [HttpGet("user-reviews/{userId}")]
        [Authorize(Policy = AuthorizationRoles.BuyerOrAdmin)]
        public async Task<IActionResult> GetUserReviews([FromRoute] string userId)
        {
            try
            {
                var review = _reviewManager.GetUserReviewsResponse(userId);

                return Ok(review);
            }
            catch (Exception e)
            {
                return BadRequest("couldn't get the user reviews");
            }
        }

        [HttpGet("event-reviews/{eventId}")]
        [Authorize(Policy = AuthorizationRoles.OrganizerOrAdmin)]
        public async Task<IActionResult> GetEventReviews([FromRoute] string eventId)
        {
            try
            {
                var review = _reviewManager.GetEventReviewsResponse(eventId);

                return Ok(review);
            }
            catch (Exception e)
            {
                return BadRequest("couldn't get the event reviews");
            }
        }

        [HttpPost("create-review")]
        [Authorize(Policy = AuthorizationRoles.BuyerOrAdmin)]
        public async Task<IActionResult> Create([FromBody] ReviewModel reviewModel)
        {
            try
            {
                _reviewManager.Create(reviewModel);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest("couldn't create the review");
            }
        }

        [HttpPatch("update-review")]
        [Authorize(Policy = AuthorizationRoles.BuyerOrAdmin)]
        public async Task<IActionResult> Update([FromBody] ReviewModel reviewModel)
        {
            try
            {
                _reviewManager.Update(reviewModel);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest("couldn't update the review");
            }
        }

        [HttpDelete("delete-review/{id}")]
        [Authorize(Policy = AuthorizationRoles.BuyerOrAdmin)]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            try
            {
                _reviewManager.Delete(id);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest("couldn't delete the review");
            }
        }
    }
}