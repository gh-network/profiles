using System;

namespace GhostNetwork.Profiles
{
    public class Profile
    {
        public Profile(Guid id,
            string firstName,
            string lastName,
            string gender,
            DateTimeOffset? dateOfBirth,
            string city,
            string profilePicture)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Gender = gender;
            DateOfBirth = dateOfBirth;
            City = city;
            ProfilePicture = profilePicture;
        }

        public Guid Id { get; }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public string Gender { get; private set; }

        public DateTimeOffset? DateOfBirth { get; private set; }

        public string City { get; private set; }
        
        public string ProfilePicture { get; private set; }

        public void Update(string firstName,
            string lastName,
            string gender,
            DateTimeOffset? dateOfBirth,
            string city,
            string profilePicture)
        {
            FirstName = firstName;
            LastName = lastName;
            Gender = gender;
            DateOfBirth = dateOfBirth;
            City = city;
            ProfilePicture = profilePicture;
        }
    }
}
