using System;
using System.Collections.Generic;
using System.Text;

namespace GhostNetwork.Profiles.FriendsFuntionality
{
    public class Friend
    {
        public Friend(Guid id, Guid userId, Guid friendId)
        {
            Id = id;
            UserId = userId;
            FriendId = friendId;
        }

        public Guid Id { get; }

        public Guid UserId { get; private set; }

        public Guid FriendId { get; private set; }
    }
}
