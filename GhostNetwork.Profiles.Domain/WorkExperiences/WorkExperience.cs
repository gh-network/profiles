using System;

namespace GhostNetwork.Profiles.WorkExperiences
{
    public class WorkExperience
    {
        public WorkExperience(string id, string profileId, string companyName, string description, DateTimeOffset? startWork, DateTimeOffset? finishWork)
        {
            Id = id;
            ProfileId = profileId;
            StartWork = startWork;
            FinishWork = finishWork;
            CompanyName = companyName;
            Description = description;
        }

        public string Id { get; }

        public string Description { get; set; }

        public string CompanyName { get; private set; }

        public DateTimeOffset? StartWork { get; private set; }

        public DateTimeOffset? FinishWork { get; private set; }

        public string ProfileId { get; }

        public void Update(string companyName, string description, DateTimeOffset? startWork, DateTimeOffset? finishWork)
        {
            CompanyName = companyName;
            StartWork = startWork;
            FinishWork = finishWork;
            Description = description;
        }
    }
}
