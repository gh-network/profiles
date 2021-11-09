using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GhostNetwork.Profiles.Api.Helpers;
using GhostNetwork.Profiles.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GhostNetwork.Profiles.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        private readonly IProfileService profileService;

        public ProfilesController(IProfileService profileService)
        {
            this.profileService = profileService;
        }

        /// <summary>
        /// Search profiles by ids
        /// </summary>
        /// <param name="model">Profile ids</param>
        /// <response code="200">Returns profile</response>
        [HttpPost("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Profile>>> SearchByIdsAsync([FromBody] ProfilesQueryModel model)
        {
            var profiles = await profileService.SearchByIdsAsync(model.Ids);

            return Ok(profiles);
        }

        /// <summary>
        /// Get profile by id
        /// </summary>
        /// <param name="id">Profile id</param>
        /// <response code="200">Returns profile</response>
        /// <response code="404">Profile not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Profile>> GetByIdAsync([FromRoute] Guid id)
        {
            var profile = await profileService.GetByIdAsync(id);

            if (profile == null)
            {
                return NotFound();
            }

            return Ok(profile);
        }

        /// <summary>
        /// Create one profile
        /// </summary>
        /// <param name="createModel">Profile</param>
        /// <response code="201">Profile successfully created</response>
        /// <response code="400">Validation failed</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateAsync([FromBody] ProfileCreateViewModel createModel)
        {
            var (result, profile) = await profileService.CreateAsync(createModel.Id, createModel.FirstName, createModel.LastName, createModel.Gender, createModel.DateOfBirth, createModel.City);

            if (result.Successed)
            {
                return Created(Url.Action("GetById", new { profile.Id }), profile);
            }

            return BadRequest(result.ToProblemDetails());
        }

        /// <summary>
        /// Update on profile
        /// </summary>
        /// <param name="id">Profile id</param>
        /// <param name="updateModel">Updated profile</param>
        /// <response code="204">Profile successfully updated</response>
        /// <response code="400">Validation failed</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Profile>> UpdateAsync([FromRoute] Guid id, [FromBody] ProfileUpdateViewModel updateModel)
        {
            var result = await profileService.UpdateAsync(id,
                updateModel.FirstName,
                updateModel.LastName,
                updateModel.Gender,
                updateModel.DateOfBirth,
                updateModel.City,
                updateModel.ProfilePicture);

            if (result.Successed)
            {
                return NoContent();
            }

            return BadRequest(result.ToProblemDetails());
        }

        /// <summary>
        /// Delete one profile
        /// </summary>
        /// <param name="id">Profile id</param>
        /// <response code="204">Profile successfully deleted</response>
        /// <response code="404">Profile not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            if (await profileService.GetByIdAsync(id) == null)
            {
                return NotFound();
            }

            await profileService.DeleteAsync(id);

            return NoContent();
        }
    }
}
