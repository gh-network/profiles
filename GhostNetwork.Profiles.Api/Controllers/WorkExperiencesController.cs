using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GhostNetwork.Profiles.Api.Helpers;
using GhostNetwork.Profiles.Api.Models;
using GhostNetwork.Profiles.WorkExperiences;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GhostNetwork.Profiles.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WorkExperiencesController : ControllerBase
    {
        private readonly IWorkExperienceService workExperienceService;
        private readonly IProfileService profileService;

        public WorkExperiencesController(IWorkExperienceService workExperienceService, IProfileService profileService)
        {
            this.workExperienceService = workExperienceService;
            this.profileService = profileService;
        }

        /// <summary>
        /// Get all profile experiences
        /// </summary>
        /// <param name="id">Work experience id</param>
        /// <response code="200">Work experience find</response>
        /// <response code="404">Work experience not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WorkExperience>> GetByIdAsync([FromRoute] Guid id)
        {
            var result = await workExperienceService.GetByIdAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        /// <summary>
        /// Get all profile experiences
        /// </summary>
        /// <param name="profileId">Profile id</param>
        /// <response code="200">Sequence of experiences</response>
        /// <response code="404">Profile not found</response>
        [HttpGet("byprofile/{profileId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<WorkExperience>>> FindByProfileAsync(Guid profileId)
        {
            if (await profileService.GetByIdAsync(profileId) == null)
            {
                return NotFound();
            }

            return Ok(await workExperienceService.FindByProfileId(profileId));
        }

        /// <summary>
        /// Create one experience
        /// </summary>
        /// <param name="model">Experience</param>
        /// <response code="201">Experience successfully created</response>
        /// <response code="400">Validation failed</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateAsync([FromBody] WorkExperienceCreateViewModel model)
        {
            var (result, id) = await workExperienceService.CreateAsync(model.CompanyName, model.Description, model.StartWork, model.FinishWork, model.ProfileId);

            if (result.Successed)
            {
                return Created(Url.Action("GetById", new { id }), await workExperienceService.GetByIdAsync(id));
            }

            return BadRequest(result.ToProblemDetails());
        }

        /// <summary>
        /// Update one experience
        /// </summary>
        /// <param name="id">Experience id</param>
        /// <param name="model">Updated experience</param>
        /// <response code="204">Experience successfully updated</response>
        /// <response code="400">Validation failed</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] WorkExperienceUpdateViewModel model)
        {
            var result = await workExperienceService.UpdateAsync(id, model.CompanyName, model.Description, model.StartWork, model.FinishWork);
            if (result.Successed)
            {
                return NoContent();
            }

            return BadRequest(result.ToProblemDetails());
        }

        /// <summary>
        /// Delete one experience
        /// </summary>
        /// <param name="id">Experience id</param>
        /// <response code="204">Experience successfully deleted</response>
        /// <response code="404">Experience not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            if (await workExperienceService.GetByIdAsync(id) == null)
            {
                return NotFound();
            }

            await workExperienceService.DeleteAsync(id);

            return NoContent();
        }
    }
}
