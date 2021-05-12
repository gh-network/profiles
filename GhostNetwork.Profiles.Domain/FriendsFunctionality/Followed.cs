using System;

namespace GhostNetwork.Profiles.FriendsFunctionality
{
    public class Followed
    {
        public Followed(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; private set; }

        public string Name { get; private set; }
    }
}