using System;
using System.Threading.Tasks;
using GhostNetwork.Profiles.Api.Helpers;
using GhostNetwork.Profiles.Api.Models;
using GhostNetwork.Profiles.SecuritySettings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GhostNetwork.Profiles.Api.Controllers
{
    public class SecuritySettingsController : ControllerBase
    {
        private readonly ISecuritySettingService securitySettingsService;

        public SecuritySettingsController(ISecuritySettingService securitySettingsService)
        {
            this.securitySettingsService = securitySettingsService;
        }

        [HttpGet("profiles/{profileId}/security-settings")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SecuritySetting>> FindByProfileAsync([FromRoute] Guid profileId)
        {
            var (result, settings) = await securitySettingsService.GetByUserIdAsync(profileId);
            if (result.Successed)
            {
                return Ok(settings);
            }

            return BadRequest(result.ToProblemDetails());
        }

        [HttpPut("profiles/{profileId}/security-settings")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateAsync([FromRoute] Guid profileId, [FromBody] SecuritySettingUpdateViewModel model)
        {
            var result = await securitySettingsService.UpdateAsync(profileId, model.AccessToPosts, model.CertainUsersForPosts,
                    model.AccessToFriends, model.CertainUsersForFriends);
            if (result.Successed)
            {
                return NoContent();
            }

            return BadRequest(result.ToProblemDetails());
        }
    }
}
