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

        Task SendFriendRequest(Guid fromUser, Guid toUser);

        Task<DomainResult> AcceptFriendRequest(Guid id);

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

        public async Task SendFriendRequest(Guid fromUser, Guid toUser)
        {
            var friend = new FriendRequest(Guid.NewGuid(), fromUser, toUser, RequestStatus.Sended);

            await friendsStorage.SendFriendRequest(friend);
        }

        public async Task<DomainResult> AcceptFriendRequest(Guid id)
        {
            var friendRequest = await friendsStorage.FindRequestById(id);

            if (friendRequest == null)
            {
                return DomainResult.Error("Friend request not found.");
            }

            friendRequest.Update(RequestStatus.Accepted);
            await friendsStorage.AcceptFriendRequest(friendRequest);
            return DomainResult.Success();
        }

        public async Task DeleteFriendRequest(Guid id)
        {
            await friendsStorage.DeleteFriendRequest(id);
        }
    }
}
