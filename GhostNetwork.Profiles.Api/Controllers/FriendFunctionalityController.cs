using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using GhostNetwork.Profiles.Api.Helpers;
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
        /// <response code="200">Return friends</response>
        [HttpGet("friends/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponseHeader(StatusCodes.Status200OK, "X-TotalCount", "Number", "Total number of user friends")]
        public async Task<ActionResult<IEnumerable<FriendRequest>>> SearchFriendsAsync(
            [FromQuery, Range(0, int.MaxValue)] int skip,
            [FromQuery, Range(1, 100)] int take,
            [FromRoute] Guid userId)
        {
            var (friends, totalCount) = await friendsService.SearchFriendsAsync(skip, take, userId);
            Response.Headers.Add("X-TotalCount", totalCount.ToString());

            return Ok(friends);
        }

        /// <summary>
        /// Search received friend requests by userId
        /// </summary>
        /// <param name="skip">Skip friends up to a specified position</param>
        /// <param name="take">Take friends up to a specified position</param>
        /// <param name="userId">Filters by userId</param>
        /// <response code="200">Return received friend requests</response>
        [HttpGet("friendrequests/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponseHeader(StatusCodes.Status200OK, "X-TotalCount", "Number", "Total number of friend requests")]
        public async Task<ActionResult<IEnumerable<FriendRequest>>> SearchFriendRequestsAsync(
            [FromQuery, Range(0, int.MaxValue)] int skip,
            [FromQuery, Range(1, 100)] int take,
            [FromRoute] Guid userId)
        {
            var (friends, totalCount) = await friendsService.SearchFriendRequestsAsync(skip, take, userId);
            Response.Headers.Add("X-TotalCount", totalCount.ToString());

            return Ok(friends);
        }

        /// <summary>
        /// Search sended friend requests by userId
        /// </summary>
        /// <param name="skip">Skip friends up to a specified position</param>
        /// <param name="take">Take friends up to a specified position</param>
        /// <param name="userId">Filters friends by userId</param>
        /// <response code="200">Return sended friend requests</response>
        [HttpGet("sendedfriendrequests/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponseHeader(StatusCodes.Status200OK, "X-TotalCount", "Number", "Total number of friend requests")]
        public async Task<ActionResult<IEnumerable<FriendRequest>>> SearchSendedFriendRequestsAsync(
            [FromQuery, Range(0, int.MaxValue)] int skip,
            [FromQuery, Range(1, 100)] int take,
            [FromRoute] Guid userId)
        {
            var (friends, totalCount) = await friendsService.SearchSentFriendRequestsAsync(skip, take, userId);
            Response.Headers.Add("X-TotalCount", totalCount.ToString());

            return Ok(friends);
        }

        /// <summary>
        /// Send friend request
        /// </summary>
        /// <param name="fromUser">Filters friend request by fromUser</param>
        /// <param name="toUser">Filters friend request by toUser</param>
        /// <response code="200">Friend request successfully created</response>
        [HttpPost("friendrequest/{fromUser}/{toUser}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<FriendRequest>> SendFriendRequest([FromRoute] Guid fromUser, [FromRoute] Guid toUser)
        {
            await friendsService.SendFriendRequestAsync(fromUser, toUser);

            return Ok();
        }

        /// <summary>
        /// Update friend request
        /// </summary>
        /// <param name="id">Friend request id</param>
        /// <param name="status">Friend request status</param>
        /// <response code="204">Request successfully updated</response>
        /// <response code="400">Smt went wrong</response>
        [HttpPut("friendrequests/{id}/{status}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateFriendRequest([FromRoute] Guid id, [FromRoute] RequestStatus status)
        {
            var result = await friendsService.UpdateFriendRequestAsync(id, status);

            if (result.Successed)
            {
                return NoContent();
            }

            return BadRequest(result.ToProblemDetails());
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
            await friendsService.DeleteFriendRequestAsync(id);
            return Ok();
        }
    }
}
