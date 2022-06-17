using System;

namespace GhostNetwork.Profiles.Api.Models
{
    public class SecuritySettingResolvingInputModel
    {
        public Guid ToUserId { get; set; }

        public string SectionName { get; set; }
    }
}
