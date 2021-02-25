using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GhostNetwork.Profiles.FriendsFuntionality
{
    public interface IFriendsFunctionalityStorage
    {
        Task<(IEnumerable<FriendRequest>, long)> FindManyFriends(int skip, int take, Guid userId);

        Task<(IEnumerable<FriendRequest>, long)> FindManyFriendRequests(int skip, int take, Guid userId);

        Task<FriendRequest> FindRequestById(Guid id);

        Task SendFriendRequest(FriendRequest friendRequest);

        Task AcceptFriendRequest(FriendRequest friendRequest);

        Task DeleteFriendRequest(Guid id);
    }
}
