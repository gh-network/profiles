using System;

namespace GhostNetwork.Profiles.WorkExperiences
{
    public class WorkExperience
    {
        public WorkExperience(string id, string profileId, DateTime startWork, DateTime? finishWork, string companyName)
        {
            Id = id;
            ProfileId = profileId;
            StartWork = startWork;
            FinishWork = finishWork;
            CompanyName = companyName;
        }

        public string Id { get; }

        public string CompanyName { get; private set; }

        public DateTime StartWork { get; private set; }

        public DateTime? FinishWork { get; private set; }

        public string ProfileId { get; }

        public void Update(string companyName, DateTime startWork, DateTime? finishWork)
        {
            CompanyName = companyName;
            StartWork = startWork;
            FinishWork = finishWork;
        }
    }
}
