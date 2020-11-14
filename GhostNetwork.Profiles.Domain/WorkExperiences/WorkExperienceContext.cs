using System;

namespace GhostNetwork.Profiles.WorkExperiences
{
    public class WorkExperienceContext
    {
        public WorkExperienceContext(string companyName, string description, DateTimeOffset? startWork, DateTimeOffset? finishWork)
        {
            CompanyName = companyName;
            Description = description;
            StartWork = startWork;
            FinishWork = finishWork;
        }

        public string CompanyName { get; }

        public string Description { get; set; }

        public DateTimeOffset? StartWork { get; }

        public DateTimeOffset? FinishWork { get; }
    }
}
