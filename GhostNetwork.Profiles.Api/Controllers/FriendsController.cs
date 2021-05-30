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
        private readonly IFriendsService friendsService;

        public FriendsController(IFriendsService friendsService)
        {
            this.friendsService = friendsService;
        }

        /// <summary>
        /// Search friends by userId
        /// </summary>
        /// <param name="skip">Skip friends up to a specified position</param>
        /// <param name="take">Take friends up to a specified position</param>
        /// <param name="id">Filters friends by userId</param>
        /// <response code="200">Return friends</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponseHeader(StatusCodes.Status200OK, "X-TotalCount", "Number", "Total number of user friends")]
        public async Task<ActionResult<IEnumerable<FriendsResponseModel>>> SearchFriendsAsync(
            [FromQuery, Range(0, int.MaxValue)] int skip,
            [FromQuery, Range(1, 100)] int take,
            [FromRoute] Guid id)
        {
            var (friends, totalCount) = await friendsService.SearchFriendsAsync(skip, take, id);
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
        [HttpGet("{id:guid}/followers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponseHeader(StatusCodes.Status200OK, "X-TotalCount", "Number", "Total number of friend requests")]
        public async Task<ActionResult<IEnumerable<FriendsResponseModel>>> SearchFollowersAsync(
            [FromQuery, Range(0, int.MaxValue)] int skip,
            [FromQuery, Range(1, 100)] int take,
            [FromRoute] Guid id)
        {
            var (friends, totalCount) = await friendsService.SearchFollowersAsync(skip, take, id);
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
        [HttpGet("{id:guid}/followed")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponseHeader(StatusCodes.Status200OK, "X-TotalCount", "Number", "Total number of friend requests")]
        public async Task<ActionResult<IEnumerable<FriendsResponseModel>>> SearchSentFriendRequestsAsync(
            [FromQuery, Range(0, int.MaxValue)] int skip,
            [FromQuery, Range(1, 100)] int take,
            [FromRoute] Guid id)
        {
            var (friends, totalCount) = await friendsService.SearchFollowedAsync(skip, take, id);
            Response.Headers.Add("X-TotalCount", totalCount.ToString());

            return Ok(friends);
        }

        /// <summary>
        /// Sent or update friend request
        /// </summary>
        /// <param name="userOne">Filters friend request by userOne</param>
        /// <param name="userTwo">Filters friend request by userTwo</param>
        /// <response code="200">Friend request successfully created</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpsertAsync([FromHeader, Required] Guid userOne, [FromHeader, Required] Guid userTwo)
        {
            if (userOne == userTwo)
            {
                return BadRequest();
            }

            await friendsService.UpsertFriendRequestAsync(userOne, userTwo);

            return Ok();
        }

        /// <summary>
        /// Delete requests
        /// </summary>
        /// <param name="userOne">UserOne</param>
        /// <param name="userTwo">UserTwo</param>
        /// <response code="200">Friend successfully deleted =( </response>
        [HttpDelete("{userOne:guid}/{userTwo:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> DeleteAsync([FromRoute] Guid userOne, [FromRoute] Guid userTwo)
        {
            await friendsService.DeleteAsync(userOne, userTwo);

            return Ok();
        }
    }
}