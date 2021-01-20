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

        Task<(DomainResult, Guid)> SendFriendRequst(Guid userId, Guid friendId);

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

        public async Task<(DomainResult, Guid)> SendFriendRequst(Guid userId, Guid friendId)
        {
            var exist = await friendsStorage.GetExistFriendRequestAsync(userId, friendId);
            if (exist != null)
            {
                return (DomainResult.Error("Allready sended request"), Guid.Empty);
            }

            var friend = new Friend(Guid.NewGuid(), userId, friendId, false);

            return (DomainResult.Success(), await friendsStorage.InsertOneAsync(friend));
        }

        public async Task DeleteOneAsync(Guid id)
        {
            await friendsStorage.DeleteOneAsync(id);
        }
    }
}
