using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
            this._reviewManager = reviewManager;
        }

        [HttpGet("reviews")]
        public async Task<IActionResult> GetReviews()
        {
            try
            {
                var reviews = _reviewManager.GetReviews();

                return Ok(reviews);
            }
            catch (Exception e)
            {
                return BadRequest("couldn't get the reviews");
            }
        }

        [HttpGet("byId/{id}")]
        public async Task<IActionResult> GetReview([FromRoute] string id)
        {
            try
            {
                var review = _reviewManager.GetReviewById(id);

                return Ok(review);
            } catch (Exception)
            {
                return BadRequest("couldn't get the review");
            }
        }

        [HttpGet("user-reviews/{userId}")]
        public async Task<IActionResult> GetUserReviews([FromRoute] string userId)
        {
            try
            {
                var review = _reviewManager.GetUserReviews(userId);

                return Ok(review);
            }
            catch (Exception e)
            {
                return BadRequest("couldn't get the user reviews");
            }
        }

        [HttpGet("event-reviews/{eventId}")]
        public async Task<IActionResult> GetEventReviews([FromRoute] string eventId)
        {
            try
            {
                var review = _reviewManager.GetEventReviews(eventId);

                return Ok(review);
            }
            catch (Exception e)
            {
                return BadRequest("couldn't get the event reviews");
            }
        }

        [HttpPost("create-review")]
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