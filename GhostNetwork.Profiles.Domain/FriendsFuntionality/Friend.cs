using System;

namespace GhostNetwork.Profiles.FriendsFuntionality
{
    public class Friend
    {
        public Friend(Guid id, Guid userId, Guid friendId, bool isFriend)
        {
            Id = id;
            UserId = userId;
            FriendId = friendId;
            IsFriend = isFriend;
        }

        public Guid Id { get; }

        public Guid UserId { get; private set; }

        public Guid FriendId { get; private set; }

        public bool IsFriend { get; private set; }

        public void Update(bool isFriend, bool isFollower)
        {
            IsFriend = isFriend;
        }
    }
}
