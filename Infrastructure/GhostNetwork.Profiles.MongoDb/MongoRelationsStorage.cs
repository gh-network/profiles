using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GhostNetwork.EventBus;
using GhostNetwork.Profiles.Friends;
using MongoDB.Driver;

namespace GhostNetwork.Profiles.MongoDb
{
    public class MongoRelationsStorage : IRelationsService
    {
        private readonly MongoDbContext context;
        private readonly IEventBus eventBus;

        public MongoRelationsStorage(MongoDbContext context, IEventBus eventBus)
        {
            this.context = context;
            this.eventBus = eventBus;
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

        public async Task<RelationType> RelationTypeAsync(Guid userId, Guid ofUserId)
        {
            var filter = Filter.Eq(p => p.ToUser, ofUserId)
                         & Filter.Eq(p => p.FromUser, userId);

            var relation = await context.FriendRequests
                .Find(filter)
                .FirstOrDefaultAsync();

            return relation?.Status switch
            {
                RequestStatus.Accepted => RelationType.Friend,
                RequestStatus.Declined => RelationType.DeclinedFollower,
                RequestStatus.Incoming => RelationType.PendingFollower,
                RequestStatus.Outgoing => RelationType.Following,
                _ => RelationType.NoRelation
            };
        }

        public async Task<bool> IsFriendAsync(Guid userId, Guid ofUserId)
        {
            var filter = Filter.Eq(p => p.ToUser, ofUserId)
                       & Filter.Eq(p => p.FromUser, userId)
                       & Filter.Eq(p => p.Status, RequestStatus.Accepted);

            return await context.FriendRequests
                .Find(filter)
                .AnyAsync();
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
                await eventBus.PublishAsync(new RequestSent(fromUser, toUser));
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
            await eventBus.PublishAsync(new RequestApproved(user, requester));
        }

        public async Task DeleteFriendAsync(Guid user, Guid friend)
        {
            await context.FriendRequests.UpdateOneAsync(
                Filter.Eq(p => p.FromUser, user)
                & Filter.Eq(p => p.ToUser, friend)
                & Filter.Eq(p => p.Status, RequestStatus.Accepted),
                Update.Set(p => p.Status, RequestStatus.Outgoing));

            await context.FriendRequests.UpdateOneAsync(
                Filter.Eq(p => p.FromUser, friend)
                & Filter.Eq(p => p.ToUser, user)
                & Filter.Eq(p => p.Status, RequestStatus.Accepted),
                Update.Set(p => p.Status, RequestStatus.Declined));

            await eventBus.PublishAsync(new Deleted(user, friend));
        }

        public async Task CancelOutgoingRequestAsync(Guid from, Guid to)
        {
            var outgoingFilter = Filter.Eq(p => p.FromUser, from)
                                 & Filter.Eq(p => p.ToUser, to)
                                 & (Filter.Eq(p => p.Status, RequestStatus.Incoming)
                                 | Filter.Eq(p => p.Status, RequestStatus.Declined));

            var incomingFilter = Filter.Eq(p => p.FromUser, to)
                                 & Filter.Eq(p => p.ToUser, from)
                                 & Filter.Eq(p => p.Status, RequestStatus.Outgoing);

            await context.FriendRequests.DeleteManyAsync(outgoingFilter | incomingFilter);
            await eventBus.PublishAsync(new RequestCancelled(from, to));
        }

        public async Task DeclineRequestAsync(Guid user, Guid requester)
        {
            var incomingFilter = Filter.Eq(p => p.FromUser, requester)
                                 & Filter.Eq(p => p.ToUser, user)
                                 & Filter.Eq(p => p.Status, RequestStatus.Incoming);

            var declineIncoming = Update
                .Set(s => s.Status, RequestStatus.Declined);

            await context.FriendRequests.UpdateOneAsync(incomingFilter, declineIncoming);
            await eventBus.PublishAsync(new RequestDeclined(user, requester));
        }
    }
}