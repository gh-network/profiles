using System;

namespace GhostNetwork.Profiles.Domain
{
    public class ProfileContext
    {
        public ProfileContext(string firstName, string lastName, DateTime dateOfBirth, string city)
        {
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            City = city;
        }

        public string FirstName { get; }

        public string LastName { get; }

        public DateTime DateOfBirth { get; }

        public string City { get; }
    }
}
