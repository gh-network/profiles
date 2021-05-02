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

        public async Task<(IEnumerable<FriendRequest>, long)> FindManyFriendsAsync(int skip, int take, Guid userId)
        {
            var filter = Builders<FriendRequestEntity>.Filter.Eq(p => p.Status, RequestStatus.Accepted)
                & Builders<FriendRequestEntity>.Filter.Where(x => x.FromUser == userId || x.ToUser == userId);

            var totalCount = await context.FriendRequests.Find(filter).CountDocumentsAsync();

            var entities = await context.FriendRequests
                .Find(filter)
                .Skip(skip)
                .Limit(take)
                .ToListAsync();

            return (entities.Select(ToDomain), totalCount);
        }

        public async Task<(IEnumerable<FriendRequest>, long)> FindManyFriendRequestsAsync(int skip, int take, Guid userId)
        {
            var filter = Builders<FriendRequestEntity>.Filter.Eq(p => p.ToUser, userId)
                & Builders<FriendRequestEntity>.Filter.Eq(p => p.Status, RequestStatus.Sended);

            var totalCount = await context.FriendRequests.Find(filter).CountDocumentsAsync();

            var entities = await context.FriendRequests
                .Find(filter)
                .Skip(skip)
                .Limit(take)
                .ToListAsync();

            return (entities.Select(ToDomain), totalCount);
        }

        public async Task<(IEnumerable<FriendRequest>, long)> FindManySentFriendRequestsAsync(int skip, int take, Guid userId)
        {
            var filter = Builders<FriendRequestEntity>.Filter.Eq(p => p.FromUser, userId)
                & Builders<FriendRequestEntity>.Filter.Eq(p => p.Status, RequestStatus.Sended);

            var totalCount = await context.FriendRequests.Find(filter).CountDocumentsAsync();

            var entities = await context.FriendRequests
                .Find(filter)
                .Skip(skip)
                .Limit(take)
                .ToListAsync();

            return (entities.Select(ToDomain), totalCount);
        }

        public async Task<FriendRequest> FindFriendRequestByIdAsync(Guid id)
        {
            var filter = Builders<FriendRequestEntity>.Filter.Eq(p => p.Id, id)
                & Builders<FriendRequestEntity>.Filter.Eq(p => p.Status, RequestStatus.Sended);

            var entity = await context.FriendRequests.Find(filter).FirstOrDefaultAsync();

            return entity == null ? null : ToDomain(entity);
        }

        public async Task InsertFriendRequestAsync(FriendRequest friendRequest)
        {
            var filter = Builders<FriendRequestEntity>.Filter.Where(x => x.FromUser == friendRequest.FromUser && x.ToUser == friendRequest.ToUser)
                | Builders<FriendRequestEntity>.Filter.Where(x => x.FromUser == friendRequest.ToUser && x.ToUser == friendRequest.FromUser);

            var exist = await context.FriendRequests.Find(filter).FirstOrDefaultAsync();

            if (exist != null)
            {
                return ;
            }

            var entity = new FriendRequestEntity
            {
                FromUser = friendRequest.FromUser,
                ToUser = friendRequest.ToUser,
                Status = friendRequest.Status
            };

            await context.FriendRequests.InsertOneAsync(entity);
        }

        public async Task UpdateFriendRequestAsync(FriendRequest friendRequest)
        {
            var filter = Builders<FriendRequestEntity>.Filter.Eq(p => p.Id, friendRequest.Id)
                & Builders<FriendRequestEntity>.Filter.Eq(p => p.Status, RequestStatus.Sended);

            var updateStatus = Builders<FriendRequestEntity>.Update.Set(s => s.Status, friendRequest.Status);

            await context.FriendRequests.UpdateOneAsync(filter, updateStatus);
        }

        public async Task DeleteFriendRequestAsync(Guid id)
        {
            var filter = Builders<FriendRequestEntity>.Filter.Eq(p => p.Id, id);

            var friendRequest = await context.FriendRequests.Find(filter).FirstOrDefaultAsync();

            if (friendRequest == null)
            {
                return;
            }

            await context.FriendRequests.DeleteOneAsync(filter);
        }

        private static FriendRequest ToDomain(FriendRequestEntity entity)
        {
            return new FriendRequest(
                entity.Id,
                entity.FromUser,
                entity.ToUser,
                entity.Status);
        }
    }
}
