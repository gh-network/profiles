using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GhostNetwork.Profiles.Friends
{
    public interface IRelationsService
    {
        Task<(IEnumerable<Guid>, long)> SearchFriendsAsync(int skip, int take, Guid userId);

        Task<(IEnumerable<Guid>, long)> SearchFollowersAsync(int skip, int take, Guid userId);

        Task<(IEnumerable<Guid>, long)> SearchIncomingRequestsAsync(int skip, int take, Guid userId);

        Task<(IEnumerable<Guid>, long)> SearchOutgoingRequestsAsync(int skip, int take, Guid userId);

        Task<RelationType> RelationTypeAsync(Guid userId, Guid ofUserId);

        Task<bool> IsFriendAsync(Guid userId, Guid ofUserId);

        Task SendRequestAsync(Guid fromUser, Guid toUser);

        Task ApproveRequestAsync(Guid user, Guid requester);

        Task DeleteFriendAsync(Guid fromUser, Guid toUser);

        Task CancelOutgoingRequestAsync(Guid from, Guid to);

        Task DeclineRequestAsync(Guid user, Guid requester);
    }
}