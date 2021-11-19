using System;

namespace GhostNetwork.Profiles.Api.Models
{
    public class ProfileUpdateViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Gender { get; set; }

        public DateTimeOffset? DateOfBirth { get; set; }

        public string City { get; set; }

        public string ProfilePicture { get; set; }
    }
}
