using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GhostNetwork.Profiles.FriendsFuntionality;
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

        public async Task<(IEnumerable<Friend>, long)> FindManyByUserAsync(int skip, int take, Guid userId)
        {
            var filter = Builders<FriendEntity>.Filter.Eq(p => p.UserId, userId);

            var totalCount = await context.Friends.Find(filter).CountDocumentsAsync();

            var entitys = await context.Friends
                .Find(filter)
                .Skip(skip)
                .Limit(take)
                .ToListAsync();

            return (entitys.Select(ToDomain), totalCount);
        }

        public async Task<Guid> InsertOneAsync(Friend friend)
        {
            var entity = new FriendEntity
            {
                UserId = friend.UserId,
                FriendId = friend.FriendId
            };

            await context.Friends.InsertOneAsync(entity);

            return entity.Id;
        }

        public async Task DeleteOneAsync(Guid userId, Guid fiendId)
        {
            var filter = Builders<FriendEntity>.Filter.Eq(p => p.UserId, userId)
                & Builders<FriendEntity>.Filter.Eq(p => p.FriendId, fiendId);

            await context.Friends.DeleteOneAsync(filter);
        }

        private static Friend ToDomain(FriendEntity entity)
        {
            return new Friend(
                entity.Id,
                entity.UserId,
                entity.FriendId);
        }
    }
}
