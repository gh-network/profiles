using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace GhostNetwork.Profiles.FriendsFuntionality
{
    public interface IFriendsFunctionalityService
    {
        Task<(IEnumerable<Friend>, long)> SearchFriendsAsync(int skip, int take, Guid userId);

        Task<(IEnumerable<Friend>, long)> SearchFriendRequestsAsync(int skip, int take, Guid userId);

        Task<(IEnumerable<Friend>, long)> SearchSentFriendRequestsAsync(int skip, int take, Guid userId);

        Task SendFriendRequestAsync(Guid fromUser, Guid toUser);

        Task AcceptFriendRequestAsync(string requestId);

        Task DeclineFriendRequestAsync(string requestId);

        Task DeleteFriendAsync(string id);
    }

    public class FriendsFunctionalityService : IFriendsFunctionalityService
    {
        private readonly IFriendsFunctionalityStorage friendsStorage;

        public FriendsFunctionalityService(IFriendsFunctionalityStorage friendsStorage)
        {
            this.friendsStorage = friendsStorage;
        }

        public async Task<(IEnumerable<Friend>, long)> SearchFriendsAsync(int skip, int take, Guid userId)
        {
            return await friendsStorage.FindManyFriendsAsync(skip, take, userId);
        }

        public async Task<(IEnumerable<Friend>, long)> SearchFriendRequestsAsync(int skip, int take, Guid userId)
        {
            return await friendsStorage.FindManyFriendRequestsAsync(skip, take, userId);
        }

        public async Task<(IEnumerable<Friend>, long)> SearchSentFriendRequestsAsync(int skip, int take, Guid userId)
        {
            return await friendsStorage.FindManySentFriendRequestsAsync(skip, take, userId);
        }

        public async Task SendFriendRequestAsync(Guid fromUser, Guid toUser)
        {
            var friend = new Friend(fromUser.ToString() + toUser.ToString(), fromUser, toUser, RequestStatus.Sended);

            await friendsStorage.InsertFriendRequestAsync(friend);
        }

        public async Task AcceptFriendRequestAsync(string requestId)
        {
            await friendsStorage.AcceptRequestAsync(requestId);
        }

        public async Task DeclineFriendRequestAsync(string requestId)
        {
            await friendsStorage.DeclineRequestAsync(requestId);
        }

        public async Task DeleteFriendAsync(string id)
        {
            await friendsStorage.DeleteFriendAsync(id);
        }
    }
}
