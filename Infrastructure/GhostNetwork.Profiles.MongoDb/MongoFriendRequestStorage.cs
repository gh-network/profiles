using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GhostNetwork.Profiles.FriendsFuntionality;
using MongoDB.Driver;

namespace GhostNetwork.Profiles.MongoDb
{
    public class MongoFriendRequestStorage : IFriendsFunctionalityStorage
    {
        private readonly MongoDbContext context;

        public MongoFriendRequestStorage(MongoDbContext context)
        {
            this.context = context;
        }

        public async Task<(IEnumerable<Friend>, long)> FindManyFriendsAsync(int skip, int take, Guid userId)
        {
            var filter = Builders<FriendEntity>.Filter.Eq(p => p.Status, RequestStatus.Accepted)
                         & Builders<FriendEntity>.Filter.Eq(x => x.FromUser, userId)
                         | Builders<FriendEntity>.Filter.Eq(p => p.Status, RequestStatus.Accepted)
                         & Builders<FriendEntity>.Filter.Eq(x => x.ToUser, userId);

            var totalCount = await context.FriendRequests.Find(filter).CountDocumentsAsync();

            var entities = await context.FriendRequests
                .Find(filter)
                .Skip(skip)
                .Limit(take)
                .ToListAsync();

            return (entities.Select(ToDomain), totalCount);
        }

        public async Task<(IEnumerable<Friend>, long)> FindManyFriendRequestsAsync(int skip, int take, Guid userId)
        {
            var filter = Builders<FriendEntity>.Filter.Eq(p => p.ToUser, userId)
                & Builders<FriendEntity>.Filter.Eq(p => p.Status, RequestStatus.Sended);

            var totalCount = await context.FriendRequests.Find(filter).CountDocumentsAsync();

            var entities = await context.FriendRequests
                .Find(filter)
                .Skip(skip)
                .Limit(take)
                .ToListAsync();

            return (entities.Select(ToDomain), totalCount);
        }

        public async Task<(IEnumerable<Friend>, long)> FindManySentFriendRequestsAsync(int skip, int take, Guid userId)
        {
            var filter = Builders<FriendEntity>.Filter.Eq(p => p.FromUser, userId)
                & Builders<FriendEntity>.Filter.Eq(p => p.Status, RequestStatus.Sended);

            var totalCount = await context.FriendRequests.Find(filter).CountDocumentsAsync();

            var entities = await context.FriendRequests
                .Find(filter)
                .Skip(skip)
                .Limit(take)
                .ToListAsync();

            return (entities.Select(ToDomain), totalCount);
        }

        public async Task<Friend> FindFriendRequestByIdAsync(string id)
        {
            var filter = Builders<FriendEntity>.Filter.Eq(p => p.Id, id)
                & Builders<FriendEntity>.Filter.Eq(p => p.Status, RequestStatus.Sended);

            var entity = await context.FriendRequests.Find(filter).FirstOrDefaultAsync();

            return entity == null ? null : ToDomain(entity);
        }

        public async Task InsertFriendRequestAsync(Friend friendRequest)
        {
            var filter = Builders<FriendEntity>.Filter.Eq(x => x.FromUser, friendRequest.FromUser)
                         & Builders<FriendEntity>.Filter.Eq(x => x.ToUser, friendRequest.ToUser)
                         | Builders<FriendEntity>.Filter.Eq(x => x.FromUser, friendRequest.ToUser)
                         & Builders<FriendEntity>.Filter.Eq(x => x.ToUser, friendRequest.FromUser);

            var exist = await context.FriendRequests.Find(filter).FirstOrDefaultAsync();

            if (exist != null)
            {
                return ;
            }

            var entity = new FriendEntity
            {
                Id = friendRequest.Id,
                FromUser = friendRequest.FromUser,
                ToUser = friendRequest.ToUser,
                Status = RequestStatus.Sended
            };

            await context.FriendRequests.InsertOneAsync(entity);
        }

        public async Task AcceptRequestAsync(string requestId)
        {
            var filter = Builders<FriendEntity>.Filter.Eq(p => p.Id, requestId)
                         & Builders<FriendEntity>.Filter.Eq(p => p.Status, RequestStatus.Sended);
            
            var friendRequest = await context.FriendRequests.Find(filter).FirstOrDefaultAsync();

            if (friendRequest == null)
            {
                return;
            }

            var acceptRequest = Builders<FriendEntity>.Update.Set(s => s.Status, RequestStatus.Accepted);

            await context.FriendRequests.UpdateOneAsync(filter, acceptRequest);
        }

        public async Task DeclineRequestAsync(string requestId)
        {
            var filter = Builders<FriendEntity>.Filter.Eq(p => p.Id, requestId)
                         & Builders<FriendEntity>.Filter.Eq(p => p.Status, RequestStatus.Sended);

            var friendRequest = await context.FriendRequests.Find(filter).FirstOrDefaultAsync();

            if (friendRequest == null)
            {
                return;
            }
            
            await context.FriendRequests.DeleteOneAsync(filter);
        }
        
        
        public async Task DeleteFriendAsync(string id)
        {
            var filter = Builders<FriendEntity>.Filter.Eq(p => p.Id, id)
                         & Builders<FriendEntity>.Filter.Eq(p => p.Status, RequestStatus.Accepted);

            var friendRequest = await context.FriendRequests.Find(filter).FirstOrDefaultAsync();

            if (friendRequest == null)
            {
                return;
            }

            await context.FriendRequests.DeleteOneAsync(filter);
        }

        private static Friend ToDomain(FriendEntity entity)
        {
            return new Friend(
                entity.Id,
                entity.FromUser,
                entity.ToUser);
        }
    }
}
