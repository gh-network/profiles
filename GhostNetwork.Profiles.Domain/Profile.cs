using System;

namespace GhostNetwork.Profiles
{
    public class Profile
    {
        public Profile(string id, string firstName, string lastName, bool gender, DateTime dateOfBirth, string city)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Gender = gender;
            DateOfBirth = dateOfBirth;
            City = city;
        }

        public string Id { get; }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public bool Gender { get; private set; }

        public DateTime DateOfBirth { get; private set; }

        public string City { get; private set; }
    }
}
