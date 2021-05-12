using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GhostNetwork.Profiles.FriendsFunctionality
{
    public interface IFriendsStorage
    {
        Task<(IEnumerable<Friends>, long)> FindManyFriendsAsync(int skip, int take, Guid id);

        Task<(IEnumerable<Followers>, long)> FindManyFollowersAsync(int skip, int take, Guid id);

        Task<(IEnumerable<Followed>, long)> FindManyFollowedAsync(int skip, int take, Guid id);

        Task InsertFriendRequestAsync(Friends friendRequest);

        Task UpdateFriendRequestAsync(Guid fromUser, Guid toUser);

        Task DeleteFriendAsync(Guid fromUser, Guid toUser);
    }
}
