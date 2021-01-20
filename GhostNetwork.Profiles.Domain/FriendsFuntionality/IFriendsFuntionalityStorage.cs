using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GhostNetwork.Profiles.FriendsFuntionality
{
    public interface IFriendsFuntionalityStorage
    {
        Task<(IEnumerable<Friend>, long)> FindManyFollowing(int skip, int take, Guid userId);

        Task<(IEnumerable<Friend>, long)> FindManyFollowers(int skip, int take, Guid userId);

        Task<(IEnumerable<Friend>, long)> FindManyFriends(int skip, int take, Guid userId);

        Task<Friend> GetExistFriendRequestAsync(Guid fromUserId, Guid toUserId);

        Task<Guid> InsertOneAsync(Friend friends);

        Task DeleteOneAsync(Guid id);
    }
}
