using System;
using System.Collections.Generic;

namespace GhostNetwork.Profiles.SecuritySettings
{
    public class SecuritySetting
    {
        public SecuritySetting(Guid userId, List<Guid> certainUsersForPosts, List<Guid> certainUsersForFriends)
        {
            UserId = userId;
            CertainUsersForPosts = certainUsersForPosts;
            CertainUsersForFriends = certainUsersForFriends;
        }

        public Guid UserId { get; }

        public List<Guid> CertainUsersForPosts { get; private set; }

        public List<Guid> CertainUsersForFriends { get; private set; }

        public SecuritySetting Update(List<Guid> certainUsersForPosts, List<Guid> certainUsersForFriends)
        {
            CertainUsersForPosts = certainUsersForPosts;
            CertainUsersForFriends = certainUsersForFriends;
            return this;
        }
    }
}
