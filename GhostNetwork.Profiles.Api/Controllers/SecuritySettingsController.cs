using System;
using System.Linq;
using System.Threading.Tasks;
using GhostNetwork.Profiles.Api.Helpers;
using GhostNetwork.Profiles.Api.Models;
using GhostNetwork.Profiles.SecuritySettings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GhostNetwork.Profiles.Api.Controllers
{
    [ApiController]
    public class SecuritySettingsController : ControllerBase
    {
        private readonly ISecuritySettingService securitySettingsService;

        public SecuritySettingsController(ISecuritySettingService securitySettingsService)
        {
            this.securitySettingsService = securitySettingsService;
        }

        [HttpGet("profiles/{userId:guid}/security-settings")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SecuritySetting>> FindByProfileAsync([FromRoute] Guid userId)
        {
            var settings = await securitySettingsService.GetByUserIdAsync(userId);
            if (settings != null)
            {
                return Ok(settings);
            }

            return NotFound();
        }

        [HttpPut("profiles/{userId:guid}/security-settings")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateAsync([FromRoute] Guid userId, [FromBody] SecuritySettingUpdateViewModel model)
        {
            var result = await securitySettingsService.UpsertAsync(
                userId,
                new SecuritySettingsSection(model.Friends.Access, model.Friends.CertainUsers ?? Enumerable.Empty<Guid>()),
                new SecuritySettingsSection(model.Followers.Access, model.Followers.CertainUsers ?? Enumerable.Empty<Guid>()),
                new SecuritySettingsSection(model.Posts.Access, model.Posts.CertainUsers ?? Enumerable.Empty<Guid>()),
                new SecuritySettingsSection(model.Comments.Access, model.Comments.CertainUsers ?? Enumerable.Empty<Guid>()),
                new SecuritySettingsSection(model.ProfilePhoto.Access, model.ProfilePhoto.CertainUsers ?? Enumerable.Empty<Guid>()));

            if (result.Successed)
            {
                return NoContent();
            }

            return BadRequest(result.ToProblemDetails());
        }
    }
}
