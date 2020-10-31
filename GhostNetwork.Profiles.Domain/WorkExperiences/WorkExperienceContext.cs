using System;

namespace GhostNetwork.Profiles.Domain.WorkExperiences
{
    public class WorkExperienceContext
    {
        public WorkExperienceContext(string companyName, DateTime startWork, DateTime? finishWork)
        {
            CompanyName = companyName;
            StartWork = startWork;
            FinishWork = finishWork;
        }

        public string CompanyName { get; }

        public DateTime StartWork { get; }

        public DateTime? FinishWork { get; }
    }
}
