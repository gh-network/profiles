using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace GhostNetwork.Profiles.FriendsFuntionality
{
    public interface IFriendsFuntionalityService
    {
        Task<(IEnumerable<FriendRequest>, long)> SearchFriends(int skip, int take, Guid userId);

        Task<(IEnumerable<FriendRequest>, long)> SearchFriendRequests(int skip, int take, Guid userId);

        Task<Guid> SendFriendRequst(Guid fromUser, Guid toUser);
    }

    public class FriendsFuntionalityService : IFriendsFuntionalityService
    {
        private readonly IFriendsFuntionalityStorage friendsStorage;

        public FriendsFuntionalityService(IFriendsFuntionalityStorage friendsStorage)
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

        public async Task<Guid> SendFriendRequst(Guid fromUser, Guid toUser)
        {
            var friend = new FriendRequest(Guid.NewGuid(), fromUser, toUser, RequestStatus.Sended);

            return await friendsStorage.SendFriendRequest(friend);
        }
    }
}
