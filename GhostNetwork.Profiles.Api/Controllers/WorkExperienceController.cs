using System.Collections.Generic;
using System.Threading.Tasks;
using GhostNetwork.Profiles.Api.Helpers;
using GhostNetwork.Profiles.Api.Models;
using GhostNetwork.Profiles.WorkExperiences;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;

namespace GhostNetwork.Profiles.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkExperienceController : ControllerBase
    {
        private readonly IWorkExperienceService workExperienceService;
        private readonly IProfileService profileService;

        public WorkExperienceController(IWorkExperienceService workExperienceService, IProfileService profileService)
        {
            this.workExperienceService = workExperienceService;
            this.profileService = profileService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WorkExperience>> GetByIdAsync([FromRoute] string id)
        {
            var workExperience = await workExperienceService.GetByIdAsync(id);

            if (workExperience != null)
            {
                return Ok(workExperience);
            }

            return NotFound();
        }

        [HttpGet("byprofile/{profileId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<WorkExperience>>> FindUserExperience(string profileId)
        {
            if (await profileService.GetByIdAsync(profileId) == null)
            {
                return BadRequest("Profile not found.");
            }

            var experience = await workExperienceService.GetAllExperienceByProfileId(profileId);

            if (experience.Any())
            {
                return Ok(experience);
            }

            return NotFound();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateAsync([FromBody] WorkExperienceCreateViewModel model)
        {
            var (result, id) = await workExperienceService.CreateAsync(model.CompanyName, model.StartWork, model.FinishWork, model.ProfileId);

            if (result.Successed)
            {
                return Created(Url.Action("GetById", new { id }), await workExperienceService.GetByIdAsync(id));
            }

            return BadRequest(result.ToProblemDetails());
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteAsync(string id)
        {
            if (await workExperienceService.GetByIdAsync(id) == null)
            {
                return NotFound();
            }

            await workExperienceService.DeleteAsync(id);
            return Ok();
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateAsync([FromRoute]string id, [FromBody] WorkExperienceUpdateViewModel model)
        {
            var result = await workExperienceService.UpdateAsync(id, model.CompanyName, model.StartWork, model.FinishWork);
            if (result.Successed)
            {
                return NoContent();
            }

            return BadRequest(result.ToProblemDetails());
        }
    }
}
