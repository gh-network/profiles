using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GhostNetwork.Profiles.MsSQL
{
    public class WorkExperienceEntity
    {
        public long Id { get; set; }

        public string CompanyName { get; set; }

        public DateTime StartWork { get; set; }

        public DateTime? FinishWork { get; set; }

        public long ProfileId { get; set; }

        public ProfileEntity Profile { get; set; }
    }
}
