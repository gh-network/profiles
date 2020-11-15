using System;

namespace GhostNetwork.Profiles.Api.Models
{
    public class WorkExperienceUpdateViewModel
    {
        public string CompanyName { get; set; }

        public string Description { get; set; }

        public DateTimeOffset? StartWork { get; set; }

        public DateTimeOffset? FinishWork { get; set; }
    }
}
