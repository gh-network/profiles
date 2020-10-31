using System;

namespace GhostNetwork.Profiles.Domain.WorkExperiences
{
    public class WorkExperience
    {
        public WorkExperience(long id, long profileId, DateTime? finishWork, DateTime startWork, string companyName)
        {
            Id = id;
            ProfileId = profileId;
            FinishWork = finishWork;
            StartWork = startWork;
            CompanyName = companyName;
        }

        public long Id { get; }

        public string CompanyName { get; private set; }

        public DateTime StartWork { get; private set; }

        public DateTime? FinishWork { get; private set; }

        public long ProfileId { get; private set; }
    }
}
