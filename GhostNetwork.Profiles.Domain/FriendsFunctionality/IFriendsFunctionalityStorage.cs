using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GhostNetwork.Profiles.FriendsFuntionality
{
    public interface IFriendsFunctionalityStorage
    {
        Task<(IEnumerable<Friend>, long)> FindManyFriendsAsync(int skip, int take, Guid userId);

        Task<(IEnumerable<Friend>, long)> FindManyFriendRequestsAsync(int skip, int take, Guid userId);

        Task<(IEnumerable<Friend>, long)> FindManySentFriendRequestsAsync(int skip, int take, Guid userId);

        Task<Friend> FindFriendRequestByIdAsync(string id);

        Task InsertFriendRequestAsync(Friend friendRequest);

        Task AcceptRequestAsync(string requestId);

        Task DeclineRequestAsync(string requestId);

        Task DeleteFriendAsync(string id);
    }
}
