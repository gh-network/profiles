using System.Threading.Tasks;
using GhostNetwork.Profiles.Api.Helpers;
using GhostNetwork.Profiles.Api.Models;
using GhostNetwork.Profiles.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GhostNetwork.Profiles.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkExperienceController : ControllerBase
    {
        private readonly IWorkExperienceService workExperienceService;

        public WorkExperienceController(IWorkExperienceService workExperienceService)
        {
            this.workExperienceService = workExperienceService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WorkExperience>> GetByIdAsync([FromRoute] long id)
        {
            var workExperience = await workExperienceService.GetByIdAsync(id);

            if (workExperienceService != null)
            {
                return Ok(workExperience);
            }

            return NotFound();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateAsync([FromBody] WorkExperienceCreateViewModel model)
        {
            var (result, id) = await workExperienceService.CreateAsync(model.CompanyName, model.StartWork, model.FinishWork,
                model.ProfileId);

            if (result.Successed)
            {
                return Created(Url.Action("GetById", new {id}), await workExperienceService.GetByIdAsync(id));
            }

            return BadRequest(result.ToProblemDetails());
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteAsync(long id)
        {
            if (await workExperienceService.GetByIdAsync(id) == null)
            {
                return NotFound();
            }

            await workExperienceService.DeleteAsync(id);
            return Ok();
        }
    }
}
