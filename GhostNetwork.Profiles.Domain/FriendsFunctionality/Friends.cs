using System;

namespace GhostNetwork.Profiles.FriendsFunctionality
{
    public class Friends
    {
        public Friends(Guid userOne, Guid userTwo)
        {
            UserOne = userOne;
            UserTwo = userTwo;
        }

        public Guid UserOne { get; private set; }

        public Guid UserTwo { get; private set; }
    }
}