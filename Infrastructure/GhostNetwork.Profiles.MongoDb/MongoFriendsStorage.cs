using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GhostNetwork.Profiles.FriendsFunctionality;
using MongoDB.Driver;

namespace GhostNetwork.Profiles.MongoDb
{
    public class MongoFriendsStorage : IFriendsStorage
    {
        private readonly MongoDbContext context;

        public MongoFriendsStorage(MongoDbContext context)
        {
            this.context = context;
        }

        public async Task<(IEnumerable<Friends>, long)> FindManyFriendsAsync(int skip, int take, Guid userId)
        {
            var filter = Builders<FriendsEntity>.Filter.Eq(p => p.Status, RequestStatus.Accepted)
                         & Builders<FriendsEntity>.Filter.Eq(x => x.FromUser, userId)
                         | Builders<FriendsEntity>.Filter.Eq(p => p.Status, RequestStatus.Accepted)
                         & Builders<FriendsEntity>.Filter.Eq(x => x.ToUser, userId);

            var totalCount = await context.Friends.Find(filter).CountDocumentsAsync();

            var entities = await context.Friends
                .Find(filter)
                .Skip(skip)
                .Limit(take)
                .ToListAsync();

            return (entities.Select(ToDomain), totalCount);
        }

        public async Task<(IEnumerable<Friends>, long)> FindManyFriendRequestsAsync(int skip, int take, Guid userId)
        {
            var filter = Builders<FriendsEntity>.Filter.Eq(p => p.ToUser, userId)
                & Builders<FriendsEntity>.Filter.Eq(p => p.Status, RequestStatus.Sent);

            var totalCount = await context.Friends.Find(filter).CountDocumentsAsync();

            var entities = await context.Friends
                .Find(filter)
                .Skip(skip)
                .Limit(take)
                .ToListAsync();

            return (entities.Select(ToDomain), totalCount);
        }

        public async Task<(IEnumerable<Friends>, long)> FindManySentFriendRequestsAsync(int skip, int take, Guid userId)
        {
            var filter = Builders<FriendsEntity>.Filter.Eq(p => p.FromUser, userId)
                & Builders<FriendsEntity>.Filter.Eq(p => p.Status, RequestStatus.Sent);

            var totalCount = await context.Friends.Find(filter).CountDocumentsAsync();

            var entities = await context.Friends
                .Find(filter)
                .Skip(skip)
                .Limit(take)
                .ToListAsync();

            return (entities.Select(ToDomain), totalCount);
        }

        public async Task<Friends> FindFriendRequestByIdAsync(string id)
        {
            var filter = Builders<FriendsEntity>.Filter.Eq(p => p.Status, RequestStatus.Sent);

            var entity = await context.Friends.Find(filter).FirstOrDefaultAsync();

            return entity == null ? null : ToDomain(entity);
        }

        public async Task InsertFriendRequestAsync(Friends friendRequest)
        {
            var filter = Builders<FriendsEntity>.Filter.Eq(x => x.FromUser, friendRequest.FromUser)
                         & Builders<FriendsEntity>.Filter.Eq(x => x.ToUser, friendRequest.ToUser)
                         | Builders<FriendsEntity>.Filter.Eq(x => x.FromUser, friendRequest.ToUser)
                         & Builders<FriendsEntity>.Filter.Eq(x => x.ToUser, friendRequest.FromUser);

            var exist = await context.Friends.Find(filter).FirstOrDefaultAsync();

            if (exist != null)
            {
                return;
            }
            
            if (friendRequest.FromUser == friendRequest.ToUser)
            {
                return;
            }

            var entity = new FriendsEntity
            {
                FromUser = friendRequest.FromUser,
                ToUser = friendRequest.ToUser,
                Status = RequestStatus.Sent
            };

            await context.Friends.InsertOneAsync(entity);
        }
        
        //Now user1 and user2 can accept the request, need to fix
        public async Task UpdateFriendRequestAsync(Guid fromUser, Guid toUser)
        {
            var filter = Builders<FriendsEntity>.Filter.Eq(x => x.FromUser, fromUser)
                         & Builders<FriendsEntity>.Filter.Eq(x => x.ToUser, toUser)
                         & Builders<FriendsEntity>.Filter.Eq(x => x.Status, RequestStatus.Sent);

            var update = Builders<FriendsEntity>.Update.Set(p => p.Status, RequestStatus.Accepted);

            await context.Friends.UpdateOneAsync(filter, update);
        }

        public async Task DeleteFriendAsync(Guid fromUser, Guid toUser)
        {
            var filter = Builders<FriendsEntity>.Filter.Eq(p => p.FromUser, fromUser)
                         & Builders<FriendsEntity>.Filter.Eq(p => p.ToUser, toUser)
                         | Builders<FriendsEntity>.Filter.Eq(p => p.FromUser, toUser)
                         & Builders<FriendsEntity>.Filter.Eq(p => p.ToUser, fromUser);

            var friendRequest = await context.Friends.Find(filter).FirstOrDefaultAsync();

            if (friendRequest == null)
            {
                return;
            }

            await context.Friends.DeleteOneAsync(filter);
        }

        private static Friends ToDomain(FriendsEntity entity)
        {
            return new Friends(
                entity.FromUser,
                entity.ToUser);
        }
    }
}
