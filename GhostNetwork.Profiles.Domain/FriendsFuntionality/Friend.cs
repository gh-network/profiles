using System;

namespace GhostNetwork.Profiles.FriendsFuntionality
{
    public class Friend
    {
        public Friend(Guid id, Guid fromUser, Guid toUser, bool isFriend, bool isFollowing, bool isFollower)
        {
            Id = id;
            FromUser = fromUser;
            ToUser = toUser;
            IsFriend = isFriend;
            IsFollowing = isFollowing;
            IsFollower = isFollower;
        }

        public Guid Id { get; }

        public Guid FromUser { get; private set; }

        public Guid ToUser { get; private set; }

        public bool IsFriend { get; private set; }

        public bool IsFollowing { get; private set; }

        public bool IsFollower { get; private set; }

        public void Update(bool isFriend, bool isFollowing, bool isFollower)
        {
            IsFriend = isFriend;
            IsFollowing = isFollowing;
            IsFollower = isFollower;
        }
    }
}
