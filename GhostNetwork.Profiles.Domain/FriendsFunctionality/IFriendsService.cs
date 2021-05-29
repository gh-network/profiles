using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GhostNetwork.Profiles.FriendsFunctionality
{
    public interface IFriendsService
    {
        Task<(IEnumerable<Response>, long)> SearchFriendsAsync(int skip, int take, Guid id);

        Task<(IEnumerable<Response>, long)> SearchFollowersAsync(int skip, int take, Guid id);

        Task<(IEnumerable<Response>, long)> SearchFollowedAsync(int skip, int take, Guid id);

        Task UpsertFriendRequestAsync(Guid userOne, Guid userTwo);

        Task DeleteAsync(Guid userOne, Guid userTwo);
    }

    public class FriendsService : IFriendsService
    {
        private readonly IFriendsStorage friendsStorage;

        public FriendsService(IFriendsStorage friendsStorage)
        {
            this.friendsStorage = friendsStorage;
        }

        public async Task<(IEnumerable<Response>, long)> SearchFriendsAsync(int skip, int take, Guid id)
        {
            return await friendsStorage.GetFriendsAsync(skip, take, id);
        }

        public async Task<(IEnumerable<Response>, long)> SearchFollowersAsync(int skip, int take, Guid id)
        {
            return await friendsStorage.GetFollowersAsync(skip, take, id);
        }

        public async Task<(IEnumerable<Response>, long)> SearchFollowedAsync(int skip, int take, Guid id)
        {
            return await friendsStorage.GetFollowedAsync(skip, take, id);
        }

        public async Task UpsertFriendRequestAsync(Guid userOne, Guid userTwo)
        {
            var friend = new Friends(userOne, userTwo);

            await friendsStorage.UpsertAsync(friend);
        }

        public async Task DeleteAsync(Guid userOne, Guid userTwo)
        {
            await friendsStorage.DeleteAsync(userOne, userTwo);
        }
    }
}