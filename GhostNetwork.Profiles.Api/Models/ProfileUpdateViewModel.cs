using System;

namespace GhostNetwork.Profiles.Api.Models
{
    public class ProfileUpdateViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool Gender { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string City { get; set; }
    }
}
