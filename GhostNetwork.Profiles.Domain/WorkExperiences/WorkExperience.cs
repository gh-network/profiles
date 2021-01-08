using System;

namespace GhostNetwork.Profiles.WorkExperiences
{
    public class WorkExperience
    {
        public WorkExperience(Guid id, string companyName, string description, DateTimeOffset? startWork, DateTimeOffset? finishWork, Guid profileId)
        {
            Id = id;
            CompanyName = companyName;
            Description = description;
            StartWork = startWork;
            FinishWork = finishWork;
            ProfileId = profileId;
        }

        public Guid Id { get; }

        public string CompanyName { get; private set; }

        public string Description { get; private set; }

        public DateTimeOffset? StartWork { get; private set; }

        public DateTimeOffset? FinishWork { get; private set; }

        public Guid ProfileId { get; }

        public void Update(string companyName, string description, DateTimeOffset? startWork, DateTimeOffset? finishWork)
        {
            CompanyName = companyName;
            Description = description;
            StartWork = startWork;
            FinishWork = finishWork;
        }
    }
}
