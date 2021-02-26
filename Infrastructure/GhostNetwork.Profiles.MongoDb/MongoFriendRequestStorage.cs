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

        public async Task<(IEnumerable<FriendRequest>, long)> FindManyFriends(int skip, int take, Guid userId)
        {
            var filter = Builders<FriendRequestEntity>.Filter.Eq(p => p.ToUser, userId)
                & Builders<FriendRequestEntity>.Filter.Eq(p => p.Status, RequestStatus.Accepted)
                | Builders<FriendRequestEntity>.Filter.Eq(q => q.FromUser, userId)
                & Builders<FriendRequestEntity>.Filter.Eq(p => p.Status, RequestStatus.Accepted);

            var totalCount = await context.FriendRequests.Find(filter).CountDocumentsAsync();

            var entitys = await context.FriendRequests
                .Find(filter)
                .Skip(skip)
                .Limit(take)
                .ToListAsync();

            return (entitys.Select(ToDomain), totalCount);
        }

        public async Task<(IEnumerable<FriendRequest>, long)> FindManyFriendRequests(int skip, int take, Guid userId)
        {
            var filter = Builders<FriendRequestEntity>.Filter.Eq(p => p.ToUser, userId)
                & Builders<FriendRequestEntity>.Filter.Eq(p => p.Status, RequestStatus.Sended);

            var totalCount = await context.FriendRequests.Find(filter).CountDocumentsAsync();

            var entitys = await context.FriendRequests
                .Find(filter)
                .Skip(skip)
                .Limit(take)
                .ToListAsync();

            return (entitys.Select(ToDomain), totalCount);
        }

        public async Task<(IEnumerable<FriendRequest>, long)> FindManySendedFriendRequests(int skip, int take, Guid userId)
        {
            var filter = Builders<FriendRequestEntity>.Filter.Eq(p => p.FromUser, userId)
                & Builders<FriendRequestEntity>.Filter.Eq(p => p.Status, RequestStatus.Sended);

            var totalCount = await context.FriendRequests.Find(filter).CountDocumentsAsync();

            var entitys = await context.FriendRequests
                .Find(filter)
                .Skip(skip)
                .Limit(take)
                .ToListAsync();

            return (entitys.Select(ToDomain), totalCount);
        }

        public async Task<FriendRequest> FindRequestById(Guid id)
        {
            var filter = Builders<FriendRequestEntity>.Filter.Eq(p => p.Id, id)
                & Builders<FriendRequestEntity>.Filter.Eq(p => p.Status, RequestStatus.Sended);

            var entity = await context.FriendRequests.Find(filter).FirstOrDefaultAsync();

            return entity == null ? null : ToDomain(entity);
        }

        public async Task SendFriendRequest(FriendRequest friendRequest)
        {
            var filter = Builders<FriendRequestEntity>.Filter.Eq(p => p.FromUser, friendRequest.FromUser)
                & Builders<FriendRequestEntity>.Filter.Eq(p => p.ToUser, friendRequest.ToUser);

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

        public async Task UpdateFriendRequest(FriendRequest friendRequest)
        {
            var filter = Builders<FriendRequestEntity>.Filter.Eq(p => p.Id, friendRequest.Id)
                & Builders<FriendRequestEntity>.Filter.Eq(p => p.Status, RequestStatus.Sended);

            var updateStatus = Builders<FriendRequestEntity>.Update.Set(s => s.Status, friendRequest.Status);

            await context.FriendRequests.UpdateOneAsync(filter, updateStatus);
        }

        public async Task DeleteFriendRequest(Guid id)
        {
            var filter = Builders<FriendRequestEntity>.Filter.Eq(p => p.Id, id)
                & Builders<FriendRequestEntity>.Filter.Eq(p => p.Status, RequestStatus.Sended);

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
