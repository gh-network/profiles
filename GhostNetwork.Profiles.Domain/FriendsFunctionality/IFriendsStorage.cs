using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GhostNetwork.Profiles.FriendsFunctionality
{
    public interface IFriendsStorage
    {
        Task<(IEnumerable<Friends>, long)> FindManyFriendsAsync(int skip, int take, Guid userId);

        Task<(IEnumerable<Friends>, long)> FindManyFriendRequestsAsync(int skip, int take, Guid userId);

        Task<(IEnumerable<Friends>, long)> FindManySentFriendRequestsAsync(int skip, int take, Guid userId);

        Task<Friends> FindFriendRequestByIdAsync(string id);

        Task InsertFriendRequestAsync(Friends friendRequest);

        Task UpdateFriendRequestAsync(Guid fromUser, Guid toUser);

        Task DeleteFriendAsync(Guid fromUser, Guid toUser);
    }
}
