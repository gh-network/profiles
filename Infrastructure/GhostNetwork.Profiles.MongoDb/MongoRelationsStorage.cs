using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GhostNetwork.Profiles.Friends;
using MongoDB.Driver;

namespace GhostNetwork.Profiles.MongoDb
{
    public class MongoRelationsStorage : IRelationsService
    {
        private readonly MongoDbContext context;

        public MongoRelationsStorage(MongoDbContext context)
        {
            this.context = context;
        }

        private static FilterDefinitionBuilder<FriendsEntity> Filter => Builders<FriendsEntity>.Filter;

        private static UpdateDefinitionBuilder<FriendsEntity> Update => Builders<FriendsEntity>.Update;

        public async Task<(IEnumerable<Guid>, long)> SearchFriendsAsync(int skip, int take, Guid userId)
        {
            var filter = Filter.Eq(p => p.ToUser, userId) & Filter.Eq(p => p.Status, RequestStatus.Accepted);

            var requestCount = await context.FriendRequests
                .Find(filter)
                .CountDocumentsAsync();

            var requests = await context.FriendRequests
                .Find(filter)
                .Skip(skip)
                .Limit(take)
                .ToListAsync();

            return (requests.Select(r => r.FromUser).ToList(), requestCount);
        }

        public async Task<(IEnumerable<Guid>, long)> SearchFollowersAsync(int skip, int take, Guid userId)
        {
            var filter = Filter.Eq(p => p.ToUser, userId)
                         & (Filter.Eq(p => p.Status, RequestStatus.Declined) |
                            Filter.Eq(p => p.Status, RequestStatus.Incoming));

            var requestCount = await context.FriendRequests
                .Find(filter)
                .CountDocumentsAsync();

            var requests = await context.FriendRequests
                .Find(filter)
                .Skip(skip)
                .Limit(take)
                .ToListAsync();

            return (requests.Select(r => r.FromUser).ToList(), requestCount);
        }

        public async Task<(IEnumerable<Guid>, long)> SearchIncomingRequestsAsync(int skip, int take, Guid userId)
        {
            var filter = Filter.Eq(p => p.ToUser, userId) & Filter.Eq(p => p.Status, RequestStatus.Incoming);

            var requestCount = await context.FriendRequests
                .Find(filter)
                .CountDocumentsAsync();

            var requests = await context.FriendRequests
                .Find(filter)
                .Skip(skip)
                .Limit(take)
                .ToListAsync();

            return (requests.Select(r => r.FromUser).ToList(), requestCount);
        }

        public async Task<(IEnumerable<Guid>, long)> SearchOutgoingRequestsAsync(int skip, int take, Guid userId)
        {
            var filter = Filter.Eq(p => p.ToUser, userId) & Filter.Eq(p => p.Status, RequestStatus.Outgoing);

            var requestCount = await context.FriendRequests
                .Find(filter)
                .CountDocumentsAsync();

            var requests = await context.FriendRequests
                .Find(filter)
                .Skip(skip)
                .Limit(take)
                .ToListAsync();

            return (requests.Select(r => r.FromUser).ToList(), requestCount);
        }

        public async Task SendRequestAsync(Guid fromUser, Guid toUser)
        {
            var filter = Filter.Eq(p => p.FromUser, fromUser) & Filter.Eq(p => p.ToUser, toUser);

            var request = await context.FriendRequests
                .Find(filter)
                .FirstOrDefaultAsync();

            if (request == null)
            {
                await context.FriendRequests.InsertManyAsync(new[]
                {
                    new FriendsEntity { FromUser = fromUser, ToUser = toUser, Status = RequestStatus.Incoming },
                    new FriendsEntity { FromUser = toUser, ToUser = fromUser, Status = RequestStatus.Outgoing }
                });
            }
        }

        public async Task ApproveRequestAsync(Guid user, Guid requester)
        {
            var filter = (Filter.Eq(p => p.FromUser, requester)
                         & Filter.Eq(p => p.ToUser, user)
                         & Filter.Eq(p => p.Status, RequestStatus.Incoming))
                         | (Filter.Eq(p => p.FromUser, user)
                         & Filter.Eq(p => p.ToUser, requester)
                         & Filter.Eq(p => p.Status, RequestStatus.Outgoing));

            var update = Update
                .Set(s => s.Status, RequestStatus.Accepted);

            await context.FriendRequests
                .UpdateManyAsync(filter, update);
        }

        public async Task DeleteFriendAsync(Guid user, Guid friend)
        {
            var deleteFilter = Filter.Eq(p => p.FromUser, user)
                               & Filter.Eq(p => p.ToUser, friend)
                               & (Filter.Eq(p => p.Status, RequestStatus.Declined)
                                  | Filter.Eq(p => p.Status, RequestStatus.Accepted));

            var updateFilter = Filter.Eq(p => p.FromUser, friend)
                               & Filter.Eq(p => p.ToUser, user)
                               & Filter.Eq(p => p.Status, RequestStatus.Accepted);
            var updateToFollowers = Update.Set(p => p.Status, RequestStatus.Declined);

            await context.FriendRequests.DeleteOneAsync(deleteFilter);
            await context.FriendRequests.UpdateOneAsync(updateFilter, updateToFollowers);
        }

        public async Task CancelOutgoingRequestAsync(Guid from, Guid to)
        {
            var outgoingFilter = Filter.Eq(p => p.FromUser, from)
                                 & Filter.Eq(p => p.ToUser, to)
                                 & Filter.Eq(p => p.Status, RequestStatus.Incoming);

            var incomingFilter = Filter.Eq(p => p.FromUser, to)
                                 & Filter.Eq(p => p.ToUser, from)
                                 & Filter.Eq(p => p.Status, RequestStatus.Outgoing);

            await context.FriendRequests.DeleteManyAsync(outgoingFilter | incomingFilter);
        }

        public async Task DeclineRequestAsync(Guid user, Guid requester)
        {
            var incomingFilter = Filter.Eq(p => p.FromUser, requester)
                                 & Filter.Eq(p => p.ToUser, user)
                                 & Filter.Eq(p => p.Status, RequestStatus.Incoming);

            var declineIncoming = Update
                .Set(s => s.Status, RequestStatus.Declined);

            await context.FriendRequests.UpdateOneAsync(incomingFilter, declineIncoming);
        }
    }
}