using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace GhostNetwork.Profiles.FriendsFuntionality
{
    public interface IFriendsFunctionalityService
    {
        Task<(IEnumerable<FriendRequest>, long)> SearchFriendsAsync(int skip, int take, Guid userId);

        Task<(IEnumerable<FriendRequest>, long)> SearchFriendRequestsAsync(int skip, int take, Guid userId);

        Task<(IEnumerable<FriendRequest>, long)> SearchSentFriendRequestsAsync(int skip, int take, Guid userId);

        Task SendFriendRequestAsync(Guid fromUser, Guid toUser);

        Task<DomainResult> UpdateFriendRequestAsync(Guid id, RequestStatus status);

        Task DeleteFriendRequestAsync(Guid id);
    }

    public class FriendsFunctionalityService : IFriendsFunctionalityService
    {
        private readonly IFriendsFunctionalityStorage friendsStorage;

        public FriendsFunctionalityService(IFriendsFunctionalityStorage friendsStorage)
        {
            this.friendsStorage = friendsStorage;
        }

        public async Task<(IEnumerable<FriendRequest>, long)> SearchFriendsAsync(int skip, int take, Guid userId)
        {
            return await friendsStorage.FindManyFriendsAsync(skip, take, userId);
        }

        public async Task<(IEnumerable<FriendRequest>, long)> SearchFriendRequestsAsync(int skip, int take, Guid userId)
        {
            return await friendsStorage.FindManyFriendRequestsAsync(skip, take, userId);
        }

        public async Task<(IEnumerable<FriendRequest>, long)> SearchSentFriendRequestsAsync(int skip, int take, Guid userId)
        {
            return await friendsStorage.FindManySentFriendRequestsAsync(skip, take, userId);
        }

        public async Task SendFriendRequestAsync(Guid fromUser, Guid toUser)
        {
            var friend = new FriendRequest(Guid.NewGuid(), fromUser, toUser, RequestStatus.Sended);

            await friendsStorage.InsertFriendRequestAsync(friend);
        }

        public async Task<DomainResult> UpdateFriendRequestAsync(Guid id, RequestStatus status)
        {
            var request = await friendsStorage.FindFriendRequestByIdAsync(id);

            if (request == null)
            {
                return DomainResult.Error("Friend request not found.");
            }

            request.Update(status);
            await friendsStorage.UpdateFriendRequestAsync(request);
            return DomainResult.Success();
        }

        public async Task DeleteFriendRequestAsync(Guid id)
        {
            await friendsStorage.DeleteFriendRequestAsync(id);
        }
    }
}
