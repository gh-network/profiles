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
        private readonly IProfileService profileService;
        private readonly IAccessResolver accessResolver;

        public SecuritySettingsController(ISecuritySettingService securitySettingsService, IProfileService profileService, IAccessResolver accessResolver)
        {
            this.securitySettingsService = securitySettingsService;
            this.profileService = profileService;
            this.accessResolver = accessResolver;
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

        [HttpPost("profiles/{userId:guid}/security-settings/check-access")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> CheckAccess([FromRoute] Guid userId, [FromBody] SecuritySettingResolvingInputModel inputModel)
        {
            if (await profileService.GetByIdAsync(inputModel.ToUserId) == null)
            {
                return NotFound();
            }

            var result = await accessResolver.ResolveAccessAsync(userId, inputModel.ToUserId, inputModel.SectionName);

            if (result)
            {
                return NoContent();
            }

            return StatusCode(StatusCodes.Status403Forbidden);
        }

        [HttpPut("profiles/{userId:guid}/security-settings")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateAsync([FromRoute] Guid userId, [FromBody] SecuritySettingUpdateViewModel model)
        {
            var result = await securitySettingsService.UpsertAsync(
                userId,
                new SecuritySettingsSection(model.Friends?.Access ?? Access.Everyone, model.Friends?.CertainUsers ?? Enumerable.Empty<Guid>()),
                new SecuritySettingsSection(model.Followers?.Access ?? Access.Everyone, model.Followers?.CertainUsers ?? Enumerable.Empty<Guid>()),
                new SecuritySettingsSection(model.Posts?.Access ?? Access.Everyone, model.Posts?.CertainUsers ?? Enumerable.Empty<Guid>()),
                new SecuritySettingsSection(model.Comments?.Access ?? Access.Everyone, model.Comments?.CertainUsers ?? Enumerable.Empty<Guid>()),
                new SecuritySettingsSection(model.ProfilePhoto?.Access ?? Access.Everyone, model.ProfilePhoto?.CertainUsers ?? Enumerable.Empty<Guid>()));

            if (result.Successed)
            {
                return NoContent();
            }

            return BadRequest(result.ToProblemDetails());
        }
    }
}
