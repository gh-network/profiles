using System;

namespace GhostNetwork.Profiles.Api.Models
{
    public class WorkExperienceCreateViewModel
    {
        public string CompanyName { get; set; }

        public DateTime StartWork { get; set; }

        public DateTime? FinishWork { get; set; }

        public string ProfileId { get; set; }
    }
}
