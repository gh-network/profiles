using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GhostNetwork.Profiles.FriendsFuntionality
{
    public interface IFriendsFunctionalityStorage
    {
        Task<(IEnumerable<FriendRequest>, long)> FindManyFriendsAsync(int skip, int take, Guid userId);

        Task<(IEnumerable<FriendRequest>, long)> FindManyFriendRequestsAsync(int skip, int take, Guid userId);

        Task<(IEnumerable<FriendRequest>, long)> FindManySentFriendRequestsAsync(int skip, int take, Guid userId);

        Task<FriendRequest> FindFriendRequestByIdAsync(Guid id);

        Task InsertFriendRequestAsync(FriendRequest friendRequest);

        Task UpdateFriendRequestAsync(FriendRequest friendRequest);

        Task DeleteFriendRequestAsync(Guid id);
    }
}
