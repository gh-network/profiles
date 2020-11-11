using System;

namespace GhostNetwork.Profiles
{
    public class ProfileContext
    {
        public ProfileContext(string firstName, string lastName, string city, DateTime? dateOfBirth, string gender)
        {
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            City = city;
            Gender = gender;
        }

        public string FirstName { get; }

        public string LastName { get; }

        public string Gender { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string City { get; }
    }
}
