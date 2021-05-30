using System;

namespace GhostNetwork.Profiles.FriendsFunctionality
{
    public class FriendsResponseModel
    {
        public FriendsResponseModel(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; private set; }
    }
}