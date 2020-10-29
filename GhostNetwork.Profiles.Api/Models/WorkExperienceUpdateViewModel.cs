using System;

namespace GhostNetwork.Profiles.Api.Models
{
    public class WorkExperienceUpdateViewModel
    {
        public string CompanyName { get; set; }

        public DateTime StartWork { get; set; }

        public DateTime? FinishWork { get; set; }
    }
}
