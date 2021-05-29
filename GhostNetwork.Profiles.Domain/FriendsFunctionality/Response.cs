using System;

namespace GhostNetwork.Profiles.FriendsFunctionality
{
    public class Response
    {
        public Response(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; private set; }
    }
}