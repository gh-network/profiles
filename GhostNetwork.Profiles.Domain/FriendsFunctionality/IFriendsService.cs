using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GhostNetwork.Profiles.FriendsFunctionality
{
    public interface IFriendsService
    {
        Task<(IEnumerable<Friends>, long)> SearchFriendsAsync(int skip, int take, Guid userId);

        Task<(IEnumerable<Friends>, long)> SearchFriendRequestsAsync(int skip, int take, Guid userId);

        Task<(IEnumerable<Friends>, long)> SearchSentFriendRequestsAsync(int skip, int take, Guid userId);

        Task SendFriendRequestAsync(Guid fromUser, Guid toUser);

        Task AcceptFriendRequestAsync(Guid fromUser, Guid toUser);

        Task DeleteFriendAsync(Guid fromUser, Guid toUser);
    }

    public class FriendsService : IFriendsService
    {
        private readonly IFriendsStorage friendsStorage;

        public FriendsService(IFriendsStorage friendsStorage)
        {
            this.friendsStorage = friendsStorage;
        }

        public async Task<(IEnumerable<Friends>, long)> SearchFriendsAsync(int skip, int take, Guid userId)
        {
            return await friendsStorage.FindManyFriendsAsync(skip, take, userId);
        }

        public async Task<(IEnumerable<Friends>, long)> SearchFriendRequestsAsync(int skip, int take, Guid userId)
        {
            return await friendsStorage.FindManyFriendRequestsAsync(skip, take, userId);
        }

        public async Task<(IEnumerable<Friends>, long)> SearchSentFriendRequestsAsync(int skip, int take, Guid userId)
        {
            return await friendsStorage.FindManySentFriendRequestsAsync(skip, take, userId);
        }

        public async Task SendFriendRequestAsync(Guid fromUser, Guid toUser)
        {
            var friend = new Friends(fromUser, toUser);

            await friendsStorage.InsertFriendRequestAsync(friend);
        }

        public async Task AcceptFriendRequestAsync(Guid fromUser, Guid toUser)
        {
            await friendsStorage.UpdateFriendRequestAsync(fromUser, toUser);
        }

        public async Task DeleteFriendAsync(Guid fromUser, Guid toUser)
        {
            await friendsStorage.DeleteFriendAsync(fromUser, toUser);
        }
    }
}
