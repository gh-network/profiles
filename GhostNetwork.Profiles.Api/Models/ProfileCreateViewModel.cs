using System;

namespace GhostNetwork.Profiles.Api.Models
{
    public class ProfileCreateViewModel
    {
        public Guid? Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Gender { get; set; }

        public DateTimeOffset? DateOfBirth { get; set; }

        public string City { get; set; }
    }
}
