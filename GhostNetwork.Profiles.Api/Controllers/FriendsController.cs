using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using GhostNetwork.Profiles.FriendsFunctionality;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace GhostNetwork.Profiles.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FriendsController : ControllerBase
    {
        private readonly IFriendsService friendService;

        public FriendsController(IFriendsService friendService)
        {
            this.friendService = friendService;
        }

        /// <summary>
        /// Search friends by userId
        /// </summary>
        /// <param name="skip">Skip friends up to a specified position</param>
        /// <param name="take">Take friends up to a specified position</param>
        /// <param name="id">Filters friends by userId</param>
        /// <response code="200">Return friends</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponseHeader(StatusCodes.Status200OK, "X-TotalCount", "Number", "Total number of user friends")]
        public async Task<ActionResult<IEnumerable<Friends>>> SearchFriendsAsync(
            [FromQuery, Range(0, int.MaxValue)] int skip,
            [FromQuery, Range(1, 100)] int take,
            [FromRoute] Guid id)
        {
            var (friends, totalCount) = await friendService.SearchFriendsAsync(skip, take, id);
            Response.Headers.Add("X-TotalCount", totalCount.ToString());

            return Ok(friends);
        }

        /// <summary>
        /// Search received friend requests by userId
        /// </summary>
        /// <param name="skip">Skip friends up to a specified position</param>
        /// <param name="take">Take friends up to a specified position</param>
        /// <param name="id">Filters by userId</param>
        /// <response code="200">Return received friend requests</response>
        [HttpGet("{id}/followers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponseHeader(StatusCodes.Status200OK, "X-TotalCount", "Number", "Total number of friend requests")]
        public async Task<ActionResult<IEnumerable<Friends>>> SearchFriendRequestsAsync(
            [FromQuery, Range(0, int.MaxValue)] int skip,
            [FromQuery, Range(1, 100)] int take,
            [FromRoute] Guid id)
        {
            var (friends, totalCount) = await friendService.SearchFriendRequestsAsync(skip, take, id);
            Response.Headers.Add("X-TotalCount", totalCount.ToString());

            return Ok(friends);
        }

        /// <summary>
        /// Search sent friend requests by userId
        /// </summary>
        /// <param name="skip">Skip friends up to a specified position</param>
        /// <param name="take">Take friends up to a specified position</param>
        /// <param name="id">Filters friends by userId</param>
        /// <response code="200">Return sent friend requests</response>
        [HttpGet("{id}/followed")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponseHeader(StatusCodes.Status200OK, "X-TotalCount", "Number", "Total number of friend requests")]
        public async Task<ActionResult<IEnumerable<Friends>>> SearchSentFriendRequestsAsync(
            [FromQuery, Range(0, int.MaxValue)] int skip,
            [FromQuery, Range(1, 100)] int take,
            [FromRoute] Guid id)
        {
            var (friends, totalCount) = await friendService.SearchSentFriendRequestsAsync(skip, take, id);
            Response.Headers.Add("X-TotalCount", totalCount.ToString());

            return Ok(friends);
        }

        /// <summary>
        /// Send friend request
        /// </summary>
        /// <param name="fromUser">Filters friend request by fromUser</param>
        /// <param name="toUser">Filters friend request by toUser</param>
        /// <response code="200">Friend request successfully created</response>
        [HttpPost("{fromUser}/{toUser}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Friends>> SendFriendRequestAsync([FromRoute] Guid fromUser, [FromRoute] Guid toUser)
        {
            await friendService.SendFriendRequestAsync(fromUser, toUser);

            return Ok();
        }

        /// <summary>
        /// Accept friend request
        /// </summary>
        /// <param name="fromUser">Filters friend request by fromUser</param>
        /// <param name="toUser">Filters friend request by toUser</param>
        /// <response code="200">Friend request successfully created</response>
        [HttpPut("{fromUser}/{toUser}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Friends>> AcceptFriendRequestAsync([FromRoute] Guid fromUser, [FromRoute] Guid toUser)
        {
            await friendService.AcceptFriendRequestAsync(fromUser, toUser);

            return Ok();
        }

        /// <summary>
        /// Delete one friend
        /// </summary>
        /// <param name="fromUser">User1</param>
        /// <param name="toUser">User2</param>
        /// <response code="200">Friend successfully deleted =(</response>
        [HttpDelete("{fromUser}/{toUser}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> DeleteFriendAsync([FromRoute] Guid fromUser, [FromRoute] Guid toUser)
        {
            await friendService.DeleteFriendAsync(fromUser, toUser);
            return Ok();
        }
    }
}
