using System;

namespace GhostNetwork.Profiles.FriendsFuntionality
{
    public class Friend
    {
        public Friend(string id, Guid fromUser, Guid toUser)
        {
            Id = id;
            FromUser = fromUser;
            ToUser = toUser;
        }

        public string Id { get; }

        public Guid FromUser { get; private set; }

        public Guid ToUser { get; private set; }
    }
}
