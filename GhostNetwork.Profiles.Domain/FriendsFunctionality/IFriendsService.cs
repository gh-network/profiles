using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GhostNetwork.Profiles.FriendsFunctionality
{
    public interface IFriendsService
    {
        Task<(IEnumerable<Friends>, long)> SearchFriendsAsync(int skip, int take, Guid userId);

        Task<(IEnumerable<Followers>, long)> SearchFollowersAsync(int skip, int take, Guid userId);

        Task<(IEnumerable<Followed>, long)> SearchFollowedAsync(int skip, int take, Guid userId);

        Task SendFriendRequestAsync(Guid fromUser, string fromUserName, Guid toUser, string toUserName);

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

        public async Task<(IEnumerable<Followers>, long)> SearchFollowersAsync(int skip, int take, Guid userId)
        {
            return await friendsStorage.FindManyFollowersAsync(skip, take, userId);
        }

        public async Task<(IEnumerable<Followed>, long)> SearchFollowedAsync(int skip, int take, Guid userId)
        {
            return await friendsStorage.FindManyFollowedAsync(skip, take, userId);
        }

        public async Task SendFriendRequestAsync(Guid fromUser, string fromUserName, Guid toUser, string toUserName)
        {
            var friend = new Friends(fromUser, fromUserName, toUser, toUserName);

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
