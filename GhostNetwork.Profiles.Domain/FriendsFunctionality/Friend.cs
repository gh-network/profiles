using System;

namespace GhostNetwork.Profiles.FriendsFuntionality
{
    public class Friend
    {
        public Friend(string id, Guid fromUser, Guid toUser, RequestStatus status)
        {
            Id = id;
            FromUser = fromUser;
            ToUser = toUser;
            Status = status;
        }

        public string Id { get; }

        public Guid FromUser { get; private set; }

        public Guid ToUser { get; private set; }

        public RequestStatus Status { get; private set; }

        public void Update(RequestStatus status)
        {
            Status = status;
        }
    }
}
