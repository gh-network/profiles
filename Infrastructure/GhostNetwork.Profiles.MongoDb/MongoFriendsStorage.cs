using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GhostNetwork.Profiles.Friends;
using MongoDB.Driver;

namespace GhostNetwork.Profiles.MongoDb
{
    public class MongoFriendsStorage : IRelationsService
    {
        private readonly MongoDbContext context;
        private static FilterDefinitionBuilder<FriendsEntity> Filter => Builders<FriendsEntity>.Filter;
        private static UpdateDefinitionBuilder<FriendsEntity> Update => Builders<FriendsEntity>.Update;

        public MongoFriendsStorage(MongoDbContext context)
        {
            this.context = context;
        }

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
            var filter = Filter.Eq(p => p.ToUser, userId) & Filter.Eq(p => p.Status, RequestStatus.Declined);

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
                    new FriendsEntity { FromUser = fromUser, ToUser = toUser, Status = RequestStatus.Outgoing },
                    new FriendsEntity { FromUser = toUser, ToUser = fromUser, Status = RequestStatus.Incoming }
                });
            }
        }

        public async Task ApproveRequestAsync(Guid user, Guid requester)
        {
            var filter = Filter.Eq(p => p.ToUser, user)
                & Filter.Eq(p => p.FromUser, requester)
                & Filter.Eq(p => p.Status, RequestStatus.Incoming)
                | Filter.Eq(p => p.ToUser, requester)
                & Filter.Eq(p => p.FromUser, user)
                & Filter.Eq(p => p.Status, RequestStatus.Outgoing);

            var requestExists = await context.FriendRequests
                .Find(filter)
                .AnyAsync();

            if (requestExists)
            {
                var update = Update
                    .Set(s => s.Status, RequestStatus.Accepted);

                await context.FriendRequests.UpdateManyAsync(filter, update);
            }
        }

        public async Task DeleteRequestAsync(Guid fromUser, Guid toUser)
        {
            var filter = Filter.Eq(p => p.FromUser, fromUser)
                         & Filter.Eq(p => p.ToUser, toUser)
                         & Filter.Eq(p => p.Status, RequestStatus.Outgoing)
                         | Filter.Eq(p => p.FromUser, toUser)
                         & Filter.Eq(p => p.ToUser, fromUser)
                         & Filter.Eq(p => p.Status, RequestStatus.Incoming);

            await context.FriendRequests.DeleteManyAsync(filter);
        }

        public async Task DeclineRequestAsync(Guid user, Guid requester)
        {
            var incomingFilter = Filter.Eq(p => p.ToUser, user)
                                 & Filter.Eq(p => p.FromUser, requester)
                                 & Filter.Eq(p => p.Status, RequestStatus.Incoming);

            var outgoingFilter = Filter.Eq(p => p.ToUser, requester)
                                 & Filter.Eq(p => p.FromUser, user)
                                 & Filter.Eq(p => p.Status, RequestStatus.Outgoing);
            var declineOutgoing = Update
                .Set(s => s.Status, RequestStatus.Declined);

            await context.FriendRequests.DeleteOneAsync(incomingFilter);
            await context.FriendRequests.UpdateOneAsync(outgoingFilter, declineOutgoing);
        }
    }
}