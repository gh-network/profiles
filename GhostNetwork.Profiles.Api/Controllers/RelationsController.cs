using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using GhostNetwork.Profiles.Friends;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace GhostNetwork.Profiles.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RelationsController : ControllerBase
    {
        private readonly IRelationsService relationsService;

        public RelationsController(IRelationsService relationsService)
        {
            this.relationsService = relationsService;
        }

        /// <summary>
        /// Search user's friends
        /// </summary>
        /// <param name="skip">Skip friends up to a specified position</param>
        /// <param name="take">Take friends up to a specified position</param>
        /// <param name="user">Filters by user</param>
        /// <response code="200">Friend ids</response>
        [HttpGet("{user:guid}/friends")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponseHeader(StatusCodes.Status200OK, "X-TotalCount", "Number", "Total number of friends")]
        public async Task<ActionResult<IEnumerable<Guid>>> SearchFriendsAsync(
            [FromQuery, Range(0, int.MaxValue)] int skip,
            [FromQuery, Range(1, 100)] int take,
            [FromRoute] Guid user)
        {
            var (friends, totalCount) = await relationsService.SearchFriendsAsync(skip, take, user);
            Response.Headers.Add("X-TotalCount", totalCount.ToString());

            return Ok(friends);
        }

        /// <summary>
        /// Search user's followers
        /// </summary>
        /// <param name="skip">Skip followers up to a specified position</param>
        /// <param name="take">Take followers up to a specified position</param>
        /// <param name="user">Filters by user</param>
        /// <response code="200">Follower ids</response>
        [HttpGet("{user:guid}/followers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponseHeader(StatusCodes.Status200OK, "X-TotalCount", "Number", "Total number of followers")]
        public async Task<ActionResult<IEnumerable<Guid>>> SearchFollowersAsync(
            [FromQuery, Range(0, int.MaxValue)] int skip,
            [FromQuery, Range(1, 100)] int take,
            [FromRoute] Guid user)
        {
            var (followers, totalCount) = await relationsService.SearchFollowersAsync(skip, take, user);
            Response.Headers.Add("X-TotalCount", totalCount.ToString());

            return Ok(followers);
        }

        /// <summary>
        /// Search user's incoming friend requests 
        /// </summary>
        /// <param name="skip">Skip incoming requests up to a specified position</param>
        /// <param name="take">Take incoming requests up to a specified position</param>
        /// <param name="user">Filters by user</param>
        /// <response code="200">User ids who are waiting for your response</response>
        [HttpGet("{user:guid}/friends/incoming-requests")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponseHeader(StatusCodes.Status200OK, "X-TotalCount", "Number", "Total number of friend requests")]
        public async Task<ActionResult<IEnumerable<Guid>>> SearchIncomingFriendsRequestsAsync(
            [FromQuery, Range(0, int.MaxValue)] int skip,
            [FromQuery, Range(1, 100)] int take,
            [FromRoute] Guid user)
        {
            var (friends, totalCount) = await relationsService.SearchIncomingRequestsAsync(skip, take, user);
            Response.Headers.Add("X-TotalCount", totalCount.ToString());

            return Ok(friends);
        }

        /// <summary>
        /// Search user's outgoing friend requests
        /// </summary>
        /// <param name="skip">Skip outgoing requests up to a specified position</param>
        /// <param name="take">Take outgoing requests up to a specified position</param>
        /// <param name="user">Filters by user</param>
        /// <response code="200">User ids to whom you have sent friend request</response>
        [HttpGet("{user:guid}/friends/outgoing-requests")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponseHeader(StatusCodes.Status200OK, "X-TotalCount", "Number", "Total number of friend requests")]
        public async Task<ActionResult<IEnumerable<Guid>>> SearchOutgoingFriendsRequestsAsync(
            [FromQuery, Range(0, int.MaxValue)] int skip,
            [FromQuery, Range(1, 100)] int take,
            [FromRoute] Guid user)
        {
            var (friends, totalCount) = await relationsService.SearchOutgoingRequestsAsync(skip, take, user);
            Response.Headers.Add("X-TotalCount", totalCount.ToString());

            return Ok(friends);
        }

        /// <summary>
        /// Sent friend request
        /// </summary>
        /// <param name="fromUser">User who is sending request</param>
        /// <param name="toUser">User to whom the request was sent</param>
        /// <response code="204">Friend request successfully created</response>
        /// <response code="400"></response>
        [HttpPost("{fromUser:guid}/friends/{toUser:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> SendFriendRequestAsync([FromRoute] Guid fromUser, [FromRoute] Guid toUser)
        {
            if (fromUser == toUser)
            {
                return BadRequest();
            }

            await relationsService.SendRequestAsync(fromUser, toUser);

            return NoContent();
        }

        /// <summary>
        /// Remove from friends
        /// </summary>
        /// <param name="user">User who is removing other from friends</param>
        /// <param name="friend">User who will be removed from friends</param>
        /// <response code="204">Friend was removed from friends list</response>
        /// <response code="400"></response>
        [HttpDelete("{user:guid}/friends/{friend:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteFriendRequestAsync([FromRoute] Guid user, [FromRoute] Guid friend)
        {
            if (user == friend)
            {
                return BadRequest();
            }

            await relationsService.DeleteFriendAsync(user, friend);

            return NoContent();
        }

        /// <summary>
        /// Approve friend request
        /// </summary>
        /// <param name="user">User who received request</param>
        /// <param name="requester">User who sent request</param>
        /// <response code="204">Friend request approved</response>
        /// <response code="400"></response>
        [HttpPut("{user:guid}/friends/{requester:guid}/approve")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> ApproveFriendRequestAsync([FromRoute] Guid user, [FromRoute] Guid requester)
        {
            if (user == requester)
            {
                return BadRequest();
            }

            await relationsService.ApproveRequestAsync(user, requester);

            return NoContent();
        }

        /// <summary>
        /// Decline friend request
        /// </summary>
        /// <param name="user">User who received request</param>
        /// <param name="requester">User who sent request</param>
        /// <response code="204">Friend request declined</response>
        /// <response code="400"></response>
        [HttpPut("{user:guid}/friends/{requester:guid}/decline")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeclineFriendRequestAsync([FromRoute] Guid user, [FromRoute] Guid requester)
        {
            if (user == requester)
            {
                return BadRequest();
            }

            await relationsService.DeclineRequestAsync(user, requester);

            return NoContent();
        }
    }
}