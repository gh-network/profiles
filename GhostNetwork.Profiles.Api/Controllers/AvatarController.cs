using System.IO;
using System.Threading.Tasks;
using GhostNetwork.Profiles.Api.Helpers;
using GhostNetwork.Profiles.Avatars;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GhostNetwork.Profiles.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AvatarController : ControllerBase
    {
        private readonly IAvatarService avatarService;

        public AvatarController(IAvatarService avatarService)
        {
            this.avatarService = avatarService;
        }

        [HttpPost("{profileId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UploadFile(IFormFile file, [FromRoute] string profileId)
        {
            byte[] fileBytes;
            if (file.Length > 0)
            {
                await using (MemoryStream ms = new MemoryStream())
                {
                    await file.CopyToAsync(ms);
                    fileBytes = ms.ToArray();
                }
                string extension = Path.GetExtension(file.FileName);

                await avatarService.UploadAsync(fileBytes, extension, profileId);
                return Ok();
            }

            return BadRequest();
        }

        [HttpDelete("{profileId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteAsync([FromRoute] string profileId)
        {
            var result = await avatarService.DeleteAsync(profileId);
            if (result.Successed)
            {
                return Ok();
            }

            return BadRequest(result.ToProblemDetails());
        }
    }
}
