using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace GhostNetwork.Profiles.FriendsFuntionality
{
    public interface IFriendsFuntionalityService
    {
        Task<(IEnumerable<Friend>, long)> SearchFollowing(int skip, int take, Guid userId);

        Task<(IEnumerable<Friend>, long)> SearchFollowers(int skip, int take, Guid userId);

        Task<(IEnumerable<Friend>, long)> SearchFriends(int skip, int take, Guid userId);

        Task<(IEnumerable<Friend>, long)> SearchFriendRequests(int skip, int take, Guid userId);

        Task<Guid> SendFriendRequst(Guid fromUser, Guid toUser);

        Task DeleteOneAsync(Guid id);
    }

    public class FriendsFuntionalityService : IFriendsFuntionalityService
    {
        private readonly IFriendsFuntionalityStorage friendsStorage;

        public FriendsFuntionalityService(IFriendsFuntionalityStorage friendsStorage)
        {
            this.friendsStorage = friendsStorage;
        }

        public async Task<(IEnumerable<Friend>, long)> SearchFollowing(int skip, int take, Guid userId)
        {
            return await friendsStorage.FindManyFollowing(skip, take, userId);
        }

        public async Task<(IEnumerable<Friend>, long)> SearchFollowers(int skip, int take, Guid userId)
        {
            return await friendsStorage.FindManyFollowers(skip, take, userId);
        }

        public async Task<(IEnumerable<Friend>, long)> SearchFriends(int skip, int take, Guid userId)
        {
            return await friendsStorage.FindManyFriends(skip, take, userId);
        }

        public async Task<(IEnumerable<Friend>, long)> SearchFriendRequests(int skip, int take, Guid userId)
        {
            return await friendsStorage.FindManyFriendRequests(skip, take, userId);
        }

        public async Task<Guid> SendFriendRequst(Guid fromUser, Guid toUser)
        {
            var friend = new Friend(Guid.NewGuid(), fromUser, toUser, false, true, false);

            return await friendsStorage.InsertOneAsync(friend);
        }

        public async Task DeleteOneAsync(Guid id)
        {
            await friendsStorage.DeleteOneAsync(id);
        }
    }
}
