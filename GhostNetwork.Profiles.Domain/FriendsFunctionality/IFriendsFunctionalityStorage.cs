using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GhostNetwork.Profiles.FriendsFuntionality
{
    public interface IFriendsFunctionalityStorage
    {
        Task<(IEnumerable<FriendRequest>, long)> FindManyFriends(int skip, int take, Guid userId);

        Task<(IEnumerable<FriendRequest>, long)> FindManyFriendRequests(int skip, int take, Guid userId);

        Task<Guid> SendFriendRequest(FriendRequest friends);

        Task DeleteFriendRequest(Guid id);
    }
}
