using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace GhostNetwork.Profiles.FriendsFuntionality
{
    public interface IFriendsFunctionalityService
    {
        Task<(IEnumerable<FriendRequest>, long)> SearchFriends(int skip, int take, Guid userId);

        Task<(IEnumerable<FriendRequest>, long)> SearchFriendRequests(int skip, int take, Guid userId);

        Task<(IEnumerable<FriendRequest>, long)> SearchSendedFriendRequests(int skip, int take, Guid userId);

        Task SendFriendRequest(Guid fromUser, Guid toUser);

        Task<DomainResult> UpdateFriendRequest(Guid id, RequestStatus status);

        Task DeleteFriendRequest(Guid id);
    }

    public class FriendsFuntionalityService : IFriendsFunctionalityService
    {
        private readonly IFriendsFunctionalityStorage friendsStorage;

        public FriendsFuntionalityService(IFriendsFunctionalityStorage friendsStorage)
        {
            this.friendsStorage = friendsStorage;
        }

        public async Task<(IEnumerable<FriendRequest>, long)> SearchFriends(int skip, int take, Guid userId)
        {
            return await friendsStorage.FindManyFriends(skip, take, userId);
        }

        public async Task<(IEnumerable<FriendRequest>, long)> SearchFriendRequests(int skip, int take, Guid userId)
        {
            return await friendsStorage.FindManyFriendRequests(skip, take, userId);
        }

        public async Task<(IEnumerable<FriendRequest>, long)> SearchSendedFriendRequests(int skip, int take, Guid userId)
        {
            return await friendsStorage.FindManySendedFriendRequests(skip, take, userId);
        }

        public async Task SendFriendRequest(Guid fromUser, Guid toUser)
        {
            var friend = new FriendRequest(Guid.NewGuid(), fromUser, toUser, RequestStatus.Sended);

            await friendsStorage.InsertFriendRequest(friend);
        }

        public async Task<DomainResult> UpdateFriendRequest(Guid id, RequestStatus status)
        {
            var request = await friendsStorage.FindRequestById(id);

            if (request == null)
            {
                return DomainResult.Error("Friend request not found.");
            }

            request.Update(status);
            await friendsStorage.UpdateFriendRequest(request);
            return DomainResult.Success();
        }

        public async Task DeleteFriendRequest(Guid id)
        {
            await friendsStorage.DeleteFriendRequest(id);
        }
    }
}
