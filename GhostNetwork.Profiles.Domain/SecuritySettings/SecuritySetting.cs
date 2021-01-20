using System;
using System.Collections.Generic;

namespace GhostNetwork.Profiles.SecuritySettings
{
    public class SecuritySetting
    {
        public SecuritySetting(Guid id, Guid userId, Access accessToPosts, Access accessToFriends, List<Guid> certainUsersForPosts, List<Guid> certainUsersForFriends)
        {
            Id = id;
            UserId = userId;
            AccessToPosts = accessToPosts;
            AccessToFriends = accessToFriends;
            CertainUsersForPosts = certainUsersForPosts;
            CertainUsersForFriends = certainUsersForFriends;
        }

        public Guid Id { get; }

        public Guid UserId { get; }

        public Access AccessToPosts { get; private set; }

        public Access AccessToFriends { get; private set; }

        public List<Guid> CertainUsersForPosts { get; private set; }

        public List<Guid> CertainUsersForFriends { get; private set; }

        public SecuritySetting Update(Access accessToPosts, Access accessToFriends, List<Guid> certainUsersForPosts, List<Guid> certainUsersForFriends)
        {
            AccessToPosts = accessToPosts;
            AccessToFriends = accessToFriends;
            CertainUsersForPosts = certainUsersForPosts;
            CertainUsersForFriends = certainUsersForFriends;
            return this;
        }
    }
}
