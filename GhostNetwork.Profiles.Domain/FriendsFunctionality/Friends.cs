using System;

namespace GhostNetwork.Profiles.FriendsFunctionality
{
    public class Friends
    {
        public Friends(Guid fromUser, string fromUserName, Guid toUser, string toUserName)
        {
            FromUser = fromUser;
            FromUserName = fromUserName;
            ToUser = toUser;
            ToUserName = toUserName;
        }

        public Guid FromUser { get; private set; }

        public string FromUserName { get; private set; }

        public Guid ToUser { get; private set; }

        public string ToUserName { get; private set; }
    }
}
