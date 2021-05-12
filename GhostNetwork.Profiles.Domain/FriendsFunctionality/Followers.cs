using System;

namespace GhostNetwork.Profiles.FriendsFunctionality
{
    public class Followers
    {
        public Followers(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; private set; }

        public string Name { get; private set; }
    }
}