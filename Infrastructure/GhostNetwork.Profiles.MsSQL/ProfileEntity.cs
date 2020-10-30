using System;
using System.Collections.Generic;

namespace GhostNetwork.Profiles.MsSQL
{
    public class ProfileEntity
    {
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool Gender { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string City { get; set; }

        public virtual List<WorkExperienceEntity> WorkExperience { get; set; }
    }
}
