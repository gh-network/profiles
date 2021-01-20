using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using GhostNetwork.Profiles.FriendsFuntionality;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace GhostNetwork.Profiles.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FriendsController : Controller
    {
        private readonly IFriendsFuntionalityService friendsService;

        public FriendsController(IFriendsFuntionalityService friendsService)
        {
            this.friendsService = friendsService;
        }

        /// <summary>
        /// Search following by userId
        /// </summary>
        /// <param name="skip">Skip following up to a specified position</param>
        /// <param name="take">Take following up to a specified position</param>
        /// <param name="userId">Filters following by userId</param>
        /// <returns>Filtered sequence of friends</returns>
        [HttpGet("following/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponseHeader(StatusCodes.Status200OK, "X-TotalCount", "Number", "Total number of user friends")]
        public async Task<ActionResult<IEnumerable<Friend>>> SearchFollowingAsync(
            [FromQuery, Range(0, int.MaxValue)] int skip,
            [FromQuery, Range(1, 100)] int take,
            [FromRoute] Guid userId)
        {
            var (friends, totalCount) = await friendsService.SearchFollowing(skip, take, userId);
            Response.Headers.Add("X-TotalCount", totalCount.ToString());

            return Ok(friends);
        }

        /// <summary>
        /// Search friends by userId
        /// </summary>
        /// <param name="skip">Skip friends up to a specified position</param>
        /// <param name="take">Take friends up to a specified position</param>
        /// <param name="userId">Filters friends by userId</param>
        /// <returns>Filtered sequence of friends</returns>
        [HttpGet("followers/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponseHeader(StatusCodes.Status200OK, "X-TotalCount", "Number", "Total number of user friends")]
        public async Task<ActionResult<IEnumerable<Friend>>> SearchFollowersAsync(
            [FromQuery, Range(0, int.MaxValue)] int skip,
            [FromQuery, Range(1, 100)] int take,
            [FromRoute] Guid userId)
        {
            var (friends, totalCount) = await friendsService.SearchFollowers(skip, take, userId);
            Response.Headers.Add("X-TotalCount", totalCount.ToString());

            return Ok(friends);
        }

        /// <summary>
        /// Search friends by userId
        /// </summary>
        /// <param name="skip">Skip friends up to a specified position</param>
        /// <param name="take">Take friends up to a specified position</param>
        /// <param name="userId">Filters friends by userId</param>
        /// <returns>Filtered sequence of friends</returns>
        [HttpGet("friends/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponseHeader(StatusCodes.Status200OK, "X-TotalCount", "Number", "Total number of user friends")]
        public async Task<ActionResult<IEnumerable<Friend>>> SearchFriendsAsync(
            [FromQuery, Range(0, int.MaxValue)] int skip,
            [FromQuery, Range(1, 100)] int take,
            [FromRoute] Guid userId)
        {
            var (friends, totalCount) = await friendsService.SearchFriends(skip, take, userId);
            Response.Headers.Add("X-TotalCount", totalCount.ToString());

            return Ok(friends);
        }

        /// <summary>
        /// Add to friend list
        /// </summary>
        /// <param name="userId">Filters friends by userId</param>
        /// <param name="friendId">Filters friends by friendId</param>
        /// <response code="201">Profile successfully created</response>
        /// <response code="400">Validation failed</response>
        [HttpPost("friends/{userId}/{friendId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Friend>> SendFriendRequest([FromRoute] Guid userId, [FromRoute] Guid friendId)
        {
            var (result, id) = await friendsService.SendFriendRequst(userId, friendId);

            if (result.Successed)
            {
                return Created(string.Empty, "Done");
            }

            return BadRequest(result.Errors);
        }

        /// <summary>
        /// Delete one friend
        /// </summary>
        /// <param name="id">Friend id</param>
        /// <response code="204">Friend successfully deleted</response>
        /// <response code="404">Friend not found</response>
        [HttpDelete("friends/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteAsync([FromRoute] Guid id)
        {
            await friendsService.DeleteOneAsync(id);
            return Ok();
        }
    }
}
