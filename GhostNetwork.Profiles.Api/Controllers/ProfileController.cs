using System.Threading.Tasks;
using GhostNetwork.Profiles.Api.Helpers;
using GhostNetwork.Profiles.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GhostNetwork.Profiles.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService profileService;

        public ProfileController(IProfileService profileService)
        {
            this.profileService = profileService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Profile>> GetByIdAsync([FromRoute] long id)
        {
            var profile = await profileService.GetByIdAsync(id);

            if (profile != null)
            {
                return Ok(profile);
            }

            return NotFound();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateAsync([FromBody] ProfileCreateViewModel createModel)
        {
            var (result, id) = await profileService.CreateAsync(createModel.FirstName, createModel.LastName, createModel.Gender, createModel.DateOfBirth, createModel.City);

            if (result.Successed)
            {
                return Created(Url.Action("GetById", new { id }), await profileService.GetByIdAsync(id));
            }

            return BadRequest(result.ToProblemDetails());
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateAsync([FromRoute]long id, [FromBody] ProfileUpdateViewModel updateModel)
        {
            var result = await profileService.UpdateAsync(id, updateModel.FirstName, updateModel.LastName, updateModel.Gender, updateModel.DateOfBirth, updateModel.City);

            if (result.Successed)
            {
                return NoContent();
            }

            return BadRequest(result.ToProblemDetails());
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteAsync(long id)
        {
            if (await profileService.GetByIdAsync(id) == null)
            {
                return NotFound();
            }

            await profileService.DeleteAsync(id);
            return Ok();
        }
    }
}
