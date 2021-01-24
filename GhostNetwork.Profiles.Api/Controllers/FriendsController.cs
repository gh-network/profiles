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
        [SwaggerResponseHeader(StatusCodes.Status200OK, "X-TotalCount", "Number", "Total number of following")]
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
        /// Search followers by userId
        /// </summary>
        /// <param name="skip">Skip friends up to a specified position</param>
        /// <param name="take">Take friends up to a specified position</param>
        /// <param name="userId">Filters friends by userId</param>
        /// <returns>Filtered sequence of friends</returns>
        [HttpGet("followers/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponseHeader(StatusCodes.Status200OK, "X-TotalCount", "Number", "Total number of user followers")]
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
        /// Search friend requests by userId
        /// </summary>
        /// <param name="skip">Skip friends up to a specified position</param>
        /// <param name="take">Take friends up to a specified position</param>
        /// <param name="userId">Filters friends by userId</param>
        /// <returns>Filtered sequence of friends</returns>
        [HttpGet("friendrequests/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponseHeader(StatusCodes.Status200OK, "X-TotalCount", "Number", "Total number of friend requests")]
        public async Task<ActionResult<IEnumerable<Friend>>> SearchFriendRequestsAsync(
            [FromQuery, Range(0, int.MaxValue)] int skip,
            [FromQuery, Range(1, 100)] int take,
            [FromRoute] Guid userId)
        {
            var (friends, totalCount) = await friendsService.SearchFriendRequests(skip, take, userId);
            Response.Headers.Add("X-TotalCount", totalCount.ToString());

            return Ok(friends);
        }

        /// <summary>
        /// Send friend request
        /// </summary>
        /// <param name="fromUser">Filters friend request by fromUser</param>
        /// <param name="toUser">Filters friend request by toUser</param>
        /// <response code="200">Friend request successfully created</response>
        /// <response code="400">Smt went wrong</response>
        [HttpPost("friendrequest/{fromUser}/{toUser}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Friend>> SendFriendRequest([FromRoute] Guid fromUser, [FromRoute] Guid toUser)
        {
            if (await friendsService.SendFriendRequst(fromUser, toUser) != null)
            {
                return Ok();
            }

            return BadRequest();
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
