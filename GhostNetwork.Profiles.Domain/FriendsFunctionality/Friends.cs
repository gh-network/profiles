using System;

namespace GhostNetwork.Profiles.FriendsFunctionality
{
    public class Friends
    {
        public Friends(Guid fromUser, Guid toUser)
        {
            FromUser = fromUser;
            ToUser = toUser;
        }

        public Guid FromUser { get; private set; }

        public Guid ToUser { get; private set; }
    }
}
