using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TicketStore.Entities;
using TicketStore.Managers;
using TicketStore.Models;
using TicketStore.Utils;

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

        [HttpGet("byId/{userId}&{eventId}")]
        [Authorize(Policy = AuthorizationRoles.Anyone)]
        public async Task<IActionResult> GetReview([FromRoute] string userId, string eventId)
        {
            try
            {
                var (resReview, errorMessage, errorType) = _reviewManager.GetReviewResponseById(userId, eventId);
                if (resReview != null)
                {
                    return Ok(resReview);
                }

                return errorType switch
                {
                    ErrorTypes.UserFault => BadRequest(JsonConvert.SerializeObject($"Getting review by id failed. Error message: {errorMessage}")),
                    ErrorTypes.ServerFault => StatusCode(StatusCodes.Status500InternalServerError),
                    _ => NotFound()
                };
            }
            catch (Exception e)
            {
                return BadRequest(JsonConvert.SerializeObject($"Couldn't get the review. Error message: {e.Message}"));
            }
        }


        [HttpGet("reviews")]
        [Authorize(Policy = AuthorizationRoles.Admin)]
        public async Task<IActionResult> GetReviews()
        {
            try
            {
                var (resReviews, errorMessage, errorType) = _reviewManager.GetReviewsResponse();
                if (resReviews != null)
                {
                    return Ok(resReviews);
                }

                return errorType switch
                {
                    ErrorTypes.UserFault => BadRequest(JsonConvert.SerializeObject($"Couldn't get the reviews. Error message: {errorMessage}")),
                    ErrorTypes.ServerFault => StatusCode(StatusCodes.Status500InternalServerError),
                    _ => NotFound()
                };
            }
            catch (Exception e)
            {
                return BadRequest(JsonConvert.SerializeObject($"Couldn't get the reviews. Error message: {e.Message}"));
            }
        }

        [HttpGet("user-reviews/{userId}")]
        [Authorize(Policy = AuthorizationRoles.BuyerOrAdmin)]
        public async Task<IActionResult> GetUserReviews([FromRoute] string userId)
        {
            try
            {
                var (resReviews, errorMessage, errorType) = _reviewManager.GetUserReviewsResponse(userId);
                if (resReviews != null)
                {
                    return Ok(resReviews);
                }

                return errorType switch
                {
                    ErrorTypes.UserFault => BadRequest(JsonConvert.SerializeObject($"Getting review by id failed. Error message: {errorMessage}")),
                    ErrorTypes.ServerFault => StatusCode(StatusCodes.Status500InternalServerError),
                    _ => NotFound()
                };
            }
            catch (Exception e)
            {
                return BadRequest(JsonConvert.SerializeObject($"Couldn't get the user's reviews. Error message: {e.Message}"));
            }
        }

        [HttpGet("event-reviews/{eventId}")]
        [Authorize(Policy = AuthorizationRoles.OrganizerOrAdmin)]
        public async Task<IActionResult> GetEventReviews([FromRoute] string eventId)
        {
            try
            {
                var (resReviews, errorMessage, errorType) = _reviewManager.GetEventReviewsResponse(eventId);
                if (resReviews != null)
                {
                    return Ok(resReviews);
                }

                return errorType switch
                {
                    ErrorTypes.UserFault => BadRequest(JsonConvert.SerializeObject($"Getting review by id failed. Error message: {errorMessage}")),
                    ErrorTypes.ServerFault => StatusCode(StatusCodes.Status500InternalServerError),
                    _ => NotFound()
                };
            }
            catch (Exception e)
            {
                return BadRequest(JsonConvert.SerializeObject($"Couldn't get the event's reviews. Error message: {e.Message}"));
            }
        }

        [HttpPost("create-review")]
        [Authorize(Policy = AuthorizationRoles.BuyerOrAdmin)]
        public async Task<IActionResult> Create([FromBody] ReviewModel reviewModel)
        {
            try
            {
                var (success, errorMessage, errorType) = _reviewManager.Create(reviewModel);
                if (success)
                {
                    return Created("success", JsonConvert.SerializeObject("Review created successfully"));
                }

                return errorType switch
                {
                    ErrorTypes.UserFault => BadRequest(JsonConvert.SerializeObject($"Couldn't create the review. Error message: {errorMessage}")),
                    ErrorTypes.ServerFault or _ => StatusCode(StatusCodes.Status500InternalServerError)
                };
            }
            catch (Exception e)
            {
                return BadRequest(JsonConvert.SerializeObject($"Couldn't create the review. Error message: {e.Message}"));
            }
        }

        [HttpPatch("update-review")]
        [Authorize(Policy = AuthorizationRoles.BuyerOrAdmin)]
        public async Task<IActionResult> Update([FromBody] ReviewModel reviewModel)
        {
            try
            {
                var (success, errorMessage, errorType) = _reviewManager.Update(reviewModel);
                if (success)
                {
                    return Ok("Review updated successfully");
                }

                return errorType switch
                {
                    ErrorTypes.UserFault => BadRequest(JsonConvert.SerializeObject($"Couldn't update the review. Error message: {errorMessage}")),
                    ErrorTypes.ServerFault or _ => StatusCode(StatusCodes.Status500InternalServerError)
                };
            }
            catch (Exception e)
            {
                return BadRequest(JsonConvert.SerializeObject($"Couldn't update the review. Error message: {e.Message}"));
            }
        }

        [HttpDelete("delete-review/{userId}&{eventId}")]
        [Authorize(Policy = AuthorizationRoles.BuyerOrAdmin)]
        public async Task<IActionResult> Delete([FromRoute] string userId, string eventId)
        {
            try
            {
                var (success, errorMessage, errorType) = _reviewManager.Delete(userId, eventId);
                if (success)
                {
                    return Ok(JsonConvert.SerializeObject("Review deleted successfully"));
                }

                return errorType switch
                {
                    ErrorTypes.UserFault => BadRequest(JsonConvert.SerializeObject($"Couldn't delete the review. Error message: {errorMessage}")),
                    ErrorTypes.ServerFault or _ => StatusCode(StatusCodes.Status500InternalServerError)
                };
            }
            catch (Exception e)
            {
                return BadRequest(JsonConvert.SerializeObject($"Couldn't delete the review. Error message: {e.Message}"));
            }
        }
    }
}