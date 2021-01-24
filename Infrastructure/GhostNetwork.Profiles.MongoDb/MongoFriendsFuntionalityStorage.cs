using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GhostNetwork.Profiles.FriendsFuntionality;
using MongoDB.Driver;

namespace GhostNetwork.Profiles.MongoDb
{
    public class MongoFriendsFuntionalityStorage : IFriendsFuntionalityStorage
    {
        private readonly MongoDbContext context;

        public MongoFriendsFuntionalityStorage(MongoDbContext context)
        {
            this.context = context;
        }

        public async Task<(IEnumerable<Friend>, long)> FindManyFollowing(int skip, int take, Guid userId)
        {
            var filter = Builders<FriendEntity>.Filter.Eq(p => p.FromUser, userId)
                & Builders<FriendEntity>.Filter.Eq(p => p.IsFollowing, true);

            var totalCount = await context.Friends.Find(filter).CountDocumentsAsync();

            var entitys = await context.Friends
                .Find(filter)
                .Skip(skip)
                .Limit(take)
                .ToListAsync();

            return (entitys.Select(ToDomain), totalCount);
        }

        public async Task<(IEnumerable<Friend>, long)> FindManyFollowers(int skip, int take, Guid userId)
        {
            var filter = Builders<FriendEntity>.Filter.Eq(p => p.ToUser, userId)
                & Builders<FriendEntity>.Filter.Eq(p => p.IsFollower, true);

            var totalCount = await context.Friends.Find(filter).CountDocumentsAsync();

            var entitys = await context.Friends
                .Find(filter)
                .Skip(skip)
                .Limit(take)
                .ToListAsync();

            return (entitys.Select(ToDomain), totalCount);
        }

        public async Task<(IEnumerable<Friend>, long)> FindManyFriends(int skip, int take, Guid userId)
        {
            var filter = Builders<FriendEntity>.Filter.Eq(p => p.ToUser, userId)
                & Builders<FriendEntity>.Filter.Eq(p => p.IsFriends, true)
                | Builders<FriendEntity>.Filter.Eq(q => q.FromUser, userId)
                & Builders<FriendEntity>.Filter.Eq(p => p.IsFriends, true);

            var totalCount = await context.Friends.Find(filter).CountDocumentsAsync();

            var entitys = await context.Friends
                .Find(filter)
                .Skip(skip)
                .Limit(take)
                .ToListAsync();

            return (entitys.Select(ToDomain), totalCount);
        }

        public async Task<(IEnumerable<Friend>, long)> FindManyFriendRequests(int skip, int take, Guid userId)
        {
            var filter = Builders<FriendEntity>.Filter.Eq(p => p.ToUser, userId)
                & Builders<FriendEntity>.Filter.Eq(p => p.IsFollowing, true)
                & Builders<FriendEntity>.Filter.Eq(p => p.IsFriends, false);

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
            var filter = Builders<FriendEntity>.Filter.Eq(p => p.FromUser, friend.FromUser)
                & Builders<FriendEntity>.Filter.Eq(p => p.ToUser, friend.ToUser);

            var exist = await context.Friends.Find(filter).FirstOrDefaultAsync();

            if (exist != null)
            {
                return Guid.Empty;
            }

            var entity = new FriendEntity
            {
                FromUser = friend.FromUser,
                ToUser = friend.ToUser,
                IsFollowing = friend.IsFollowing
            };

            await context.Friends.InsertOneAsync(entity);

            return entity.Id;
        }

        public async Task DeleteOneAsync(Guid id)
        {
            var filter = Builders<FriendEntity>.Filter.Eq(p => p.Id, id);

            await context.Friends.DeleteOneAsync(filter);
        }

        private static Friend ToDomain(FriendEntity entity)
        {
            return new Friend(
                entity.Id,
                entity.FromUser,
                entity.ToUser,
                entity.IsFriends,
                entity.IsFollowing,
                entity.IsFollower);
        }
    }
}
