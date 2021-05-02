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
    public class FriendController : ControllerBase
    {
        private readonly IFriendsFunctionalityService friendService;

        public FriendController(IFriendsFunctionalityService friendService)
        {
            this.friendService = friendService;
        }

        /// <summary>
        /// Search friends by userId
        /// </summary>
        /// <param name="skip">Skip friends up to a specified position</param>
        /// <param name="take">Take friends up to a specified position</param>
        /// <param name="userId">Filters friends by userId</param>
        /// <response code="200">Return friends</response>
        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponseHeader(StatusCodes.Status200OK, "X-TotalCount", "Number", "Total number of user friends")]
        public async Task<ActionResult<IEnumerable<Friend>>> SearchFriendsAsync(
            [FromQuery, Range(0, int.MaxValue)] int skip,
            [FromQuery, Range(1, 100)] int take,
            [FromRoute] Guid userId)
        {
            var (friends, totalCount) = await friendService.SearchFriendsAsync(skip, take, userId);
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
        [HttpGet("followers/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponseHeader(StatusCodes.Status200OK, "X-TotalCount", "Number", "Total number of friend requests")]
        public async Task<ActionResult<IEnumerable<Friend>>> SearchFriendRequestsAsync(
            [FromQuery, Range(0, int.MaxValue)] int skip,
            [FromQuery, Range(1, 100)] int take,
            [FromRoute] Guid userId)
        {
            var (friends, totalCount) = await friendService.SearchFriendRequestsAsync(skip, take, userId);
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
        [HttpGet("followed/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponseHeader(StatusCodes.Status200OK, "X-TotalCount", "Number", "Total number of friend requests")]
        public async Task<ActionResult<IEnumerable<Friend>>> SearchSendedFriendRequestsAsync(
            [FromQuery, Range(0, int.MaxValue)] int skip,
            [FromQuery, Range(1, 100)] int take,
            [FromRoute] Guid userId)
        {
            var (friends, totalCount) = await friendService.SearchSentFriendRequestsAsync(skip, take, userId);
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
        public async Task<ActionResult<Friend>> SendFriendRequest([FromRoute] Guid fromUser, [FromRoute] Guid toUser)
        {
            await friendService.SendFriendRequestAsync(fromUser, toUser);

            return Ok();
        }

        /// <summary>
        /// Accept friend request
        /// </summary>
        /// <param name="id">Filters friend request by id</param>
        /// <response code="200">Friend request successfully accepted</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPut("accept/{id}")]
        public async Task<ActionResult> AcceptFriendRequest([FromRoute] string id)
        {
            await friendService.AcceptFriendRequestAsync(id);

            return Ok();
        }

        /// <summary>
        /// Decline one friend request
        /// </summary>
        /// <param name="id">Friend request id</param>
        /// <response code="200">Friend request successfully declined</response>
        [HttpDelete("decline/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> DeclineFriendRequest([FromRoute] string id)
        {
            await friendService.DeclineFriendRequestAsync(id);
            return Ok();
        }

        /// <summary>
        /// Delete one friend
        /// </summary>
        /// <param name="id">Friend request id</param>
        /// <response code="200">Friend successfully deleted =(</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> DeleteFriend([FromRoute] string id)
        {
            await friendService.DeleteFriendAsync(id);
            return Ok();
        }
    }
}
