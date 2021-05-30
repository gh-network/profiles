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

        public async Task<(IEnumerable<FriendsResponseModel>, long)> GetFriendsAsync(int skip, int take, Guid id)
        {
            var filter = Builders<FriendsEntity>.Filter.Eq(p => p.UserOne, id)
                         & Builders<FriendsEntity>.Filter.Eq(p => p.Status, RequestStatus.Accepted);

            var countFromSent = await context.Sent.Find(filter).CountDocumentsAsync();
            var countFromReceived = await context.Received.Find(filter).CountDocumentsAsync();

            var fromSent = await context.Sent
                .Find(filter)
                .Skip(skip)
                .Limit(take)
                .ToListAsync();

            var fromReceived = await context.Received
                .Find(filter)
                .Skip(skip)
                .Limit(take)
                .ToListAsync();

            var friends = fromSent.Concat(fromReceived).ToList();

            return (friends.Select(ToDomain), countFromSent + countFromReceived);
        }

        public async Task<(IEnumerable<FriendsResponseModel>, long)> GetFollowersAsync(int skip, int take, Guid id)
        {
            var filter = Builders<FriendsEntity>.Filter.Eq(p => p.UserOne, id)
                         & Builders<FriendsEntity>.Filter.Eq(p => p.Status, RequestStatus.Received);

            var totalCount = await context.Received.Find(filter).CountDocumentsAsync();

            var entities = await context.Received
                .Find(filter)
                .Skip(skip)
                .Limit(take)
                .ToListAsync();

            return (entities.Select(ToDomain), totalCount);
        }

        public async Task<(IEnumerable<FriendsResponseModel>, long)> GetFollowedAsync(int skip, int take, Guid id)
        {
            var filter = Builders<FriendsEntity>.Filter.Eq(p => p.UserOne, id)
                         & Builders<FriendsEntity>.Filter.Eq(p => p.Status, RequestStatus.Sent);

            var totalCount = await context.Sent.Find(filter).CountDocumentsAsync();

            var entities = await context.Sent
                .Find(filter)
                .Skip(skip)
                .Limit(take)
                .ToListAsync();

            return (entities.Select(ToDomain), totalCount);
        }

        public async Task UpsertAsync(Friends friends)
        {
            var filter = Builders<FriendsEntity>.Filter.Eq(p => p.UserOne, friends.UserOne)
                         & Builders<FriendsEntity>.Filter.Eq(p => p.UserTwo, friends.UserTwo)
                         | Builders<FriendsEntity>.Filter.Eq(p => p.UserOne, friends.UserTwo)
                         & Builders<FriendsEntity>.Filter.Eq(p => p.UserTwo, friends.UserOne);

            var sentRequest = await context.Sent.Find(filter).FirstOrDefaultAsync();
            var receivedRequest = await context.Received.Find(filter).FirstOrDefaultAsync();
            
            if (sentRequest != null && sentRequest.Status == RequestStatus.Accepted )
            {
                return;
            }
            
            if (sentRequest == null && receivedRequest == null)
            {
                var sentEntity = new FriendsEntity
                {
                    UserOne = friends.UserOne,
                    UserTwo = friends.UserTwo,
                    Status = RequestStatus.Sent
                };

                await context.Sent.InsertOneAsync(sentEntity);

                var receivedEntity = new FriendsEntity
                {
                    UserOne = friends.UserTwo,
                    UserTwo = friends.UserOne,
                    Status = RequestStatus.Received
                };

                await context.Received.InsertOneAsync(receivedEntity);
            }

            if (receivedRequest != null && friends.UserOne == receivedRequest.UserOne)
            {
                var update = Builders<FriendsEntity>.Update.Set(p => p.Status, RequestStatus.Accepted);

                await context.Sent.UpdateOneAsync(filter, update);

                await context.Received.UpdateOneAsync(filter, update);
            }
        }

        public async Task DeleteAsync(Guid userOne, Guid userTwo)
        {
            var filter = Builders<FriendsEntity>.Filter.Eq(p => p.UserOne, userOne)
                         & Builders<FriendsEntity>.Filter.Eq(p => p.UserTwo, userTwo)
                         | Builders<FriendsEntity>.Filter.Eq(p => p.UserOne, userTwo)
                         & Builders<FriendsEntity>.Filter.Eq(p => p.UserTwo, userOne);

            var exist = context.Sent.Find(filter).FirstOrDefaultAsync();
            
            if (exist == null)
            {
                return;
            }

            await context.Sent.DeleteOneAsync(filter);
            await context.Received.DeleteOneAsync(filter);
        }

        private static FriendsResponseModel ToDomain(FriendsEntity entity)
        {
            return new FriendsResponseModel(
                entity.UserTwo);
        }
    }
}