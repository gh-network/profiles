using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace GhostNetwork.Profiles.MongoDb
{
    public class MongoDbContext
    {
        private const string ProfilesCollection = "profiles";
        private const string FriendRequestsCollection = "friendRequests";
        private const string SecuritySettingsCollection = "securitySettings";

        private readonly IMongoDatabase database;

        static MongoDbContext()
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
        }

        public MongoDbContext(IMongoDatabase database)
        {
            this.database = database;
        }

        public IMongoCollection<ProfileEntity> Profiles =>
            database.GetCollection<ProfileEntity>(ProfilesCollection);

        public IMongoCollection<WorkExperienceEntity> WorkExperience =>
            database.GetCollection<WorkExperienceEntity>("workExperience");

        public IMongoCollection<SecuritySettingsEntity> SecuritySettings =>
            database.GetCollection<SecuritySettingsEntity>(SecuritySettingsCollection);

        public IMongoCollection<FriendsEntity> FriendRequests =>
            database.GetCollection<FriendsEntity>(FriendRequestsCollection);

        public async Task ConfigureAsync()
        {
            var requestIndex = Builders<FriendsEntity>.IndexKeys.Combine(
                Builders<FriendsEntity>.IndexKeys.Ascending(request => request.FromUser),
                Builders<FriendsEntity>.IndexKeys.Ascending(request => request.ToUser));
            await FriendRequests.Indexes.CreateOneAsync(new CreateIndexModel<FriendsEntity>(requestIndex, new CreateIndexOptions
            {
                Unique = true,
                Background = true
            }));
        }
    }
}
