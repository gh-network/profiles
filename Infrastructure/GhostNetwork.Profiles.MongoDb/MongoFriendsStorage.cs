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

        public async Task<(IEnumerable<Friends>, long)> FindManyFriendsAsync(int skip, int take, Guid id)
        {
            var filter = Builders<FriendsEntity>.Filter.Eq(p => p.Status, RequestStatus.Accepted)
                         & Builders<FriendsEntity>.Filter.Eq(x => x.FromUser, id)
                         | Builders<FriendsEntity>.Filter.Eq(p => p.Status, RequestStatus.Accepted)
                         & Builders<FriendsEntity>.Filter.Eq(x => x.ToUser, id);

            var totalCount = await context.Friends.Find(filter).CountDocumentsAsync();

            var entities = await context.Friends
                .Find(filter)
                .Skip(skip)
                .Limit(take)
                .ToListAsync();

            return (entities.Select(ToDomainFriends), totalCount);
        }

        public async Task<(IEnumerable<Followers>, long)> FindManyFollowersAsync(int skip, int take, Guid id)
        {
            var filter = Builders<FriendsEntity>.Filter.Eq(p => p.ToUser, id)
                & Builders<FriendsEntity>.Filter.Eq(p => p.Status, RequestStatus.Sent);

            var totalCount = await context.Friends.Find(filter).CountDocumentsAsync();

            var entities = await context.Friends
                .Find(filter)
                .Skip(skip)
                .Limit(take)
                .ToListAsync();

            return (entities.Select(ToDomainFollowers), totalCount);
        }

        public async Task<(IEnumerable<Followed>, long)> FindManyFollowedAsync(int skip, int take, Guid id)
        {
            var filter = Builders<FriendsEntity>.Filter.Eq(p => p.FromUser, id)
                & Builders<FriendsEntity>.Filter.Eq(p => p.Status, RequestStatus.Sent);

            var totalCount = await context.Friends.Find(filter).CountDocumentsAsync();

            var entities = await context.Friends
                .Find(filter)
                .Skip(skip)
                .Limit(take)
                .ToListAsync();

            return (entities.Select(ToDomainFollowed), totalCount);
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
                FromUserName = friendRequest.FromUserName,
                ToUser = friendRequest.ToUser,
                ToUserName = friendRequest.ToUserName,
                Status = RequestStatus.Sent
            };

            await context.Friends.InsertOneAsync(entity);
        }
        
        //Now user1 and user2 can accept the request, need to fix
        //Think we can validate this in Gateway if (toUser != currentUser.Id) {return;}
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

        private static Friends ToDomainFriends(FriendsEntity entity)
        {
            return new Friends(
                entity.FromUser,
                entity.FromUserName,
                entity.ToUser,
                entity.ToUserName);
        }

        private static Followed ToDomainFollowed(FriendsEntity entity)
        {
            return new Followed(
                entity.ToUser,
                entity.ToUserName);
        }
        
        private static Followers ToDomainFollowers(FriendsEntity entity)
        {
            return new Followers(
                entity.FromUser,
                entity.FromUserName);
        }
    }
}
