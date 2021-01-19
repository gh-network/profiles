using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GhostNetwork.Profiles.FriendsFuntionality
{
    public interface IFriendsStorage
    {
        Task<(IEnumerable<Friend>, long)> FindManyByUserAsync(int skip, int take, Guid userId);

        Task<Guid> InsertOneAsync(Friend friends);

        Task DeleteOneAsync(Guid userId, Guid friendId);
    }
}
