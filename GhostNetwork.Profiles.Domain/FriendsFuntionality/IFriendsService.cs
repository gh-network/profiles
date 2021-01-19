using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace GhostNetwork.Profiles.FriendsFuntionality
{
    public interface IFriendsService
    {
        Task<(IEnumerable<Friend>, long)> SearchByUser(int skip, int take, Guid userId);

        Task<(DomainResult, Guid)> AddFriendAsync(Guid userId, Guid friendId);

        Task DeleteOneAsync(Guid userId, Guid friendId);
    }

    public class FriendsService : IFriendsService
    {
        private readonly IFriendsStorage friendsStorage;

        public FriendsService(IFriendsStorage friendsStorage)
        {
            this.friendsStorage = friendsStorage;
        }

        public async Task<(IEnumerable<Friend>, long)> SearchByUser(int skip, int take, Guid userId)
        {
            return await friendsStorage.FindManyByUserAsync(skip, take, userId);
        }

        public async Task<(DomainResult, Guid)> AddFriendAsync(Guid userId, Guid friendId)
        {
            var friend = new Friend(Guid.NewGuid(), userId, friendId);

            var myId = await friendsStorage.InsertOneAsync(friend);

            return (DomainResult.Success(), myId);
        }

        public async Task DeleteOneAsync(Guid userId, Guid friendId)
        {
            await friendsStorage.DeleteOneAsync(userId, friendId);
        }
    }
}
