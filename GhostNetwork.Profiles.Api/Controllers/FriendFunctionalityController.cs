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
    public class FriendFunctionalityController : Controller
    {
        private readonly IFriendsFunctionalityService friendsService;

        public FriendFunctionalityController(IFriendsFunctionalityService friendsService)
        {
            this.friendsService = friendsService;
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
        public async Task<ActionResult<IEnumerable<FriendRequest>>> SearchFriendsAsync(
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
        public async Task<ActionResult<IEnumerable<FriendRequest>>> SearchFriendRequestsAsync(
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
        public async Task<ActionResult<FriendRequest>> SendFriendRequest([FromRoute] Guid fromUser, [FromRoute] Guid toUser)
        {
            if (await friendsService.SendFriendRequst(fromUser, toUser) != null)
            {
                return Ok();
            }

            return BadRequest();
        }

        /// <summary>
        /// Delete one friend request
        /// </summary>
        /// <param name="id">Friend request id</param>
        /// <response code="200">Friend request successfully deleted</response>
        [HttpDelete("friendrequests/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> DeleteFriendRequest([FromRoute] Guid id)
        {
            await friendsService.DeleteFriendRequest(id);
            return Ok();
        }
    }
}
