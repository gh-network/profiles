using System.Linq;
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

        public async Task MigrateGuidAsync()
        {
            var profiles = await Profiles
                .Find(Builders<ProfileEntity>.Filter.Not(Builders<ProfileEntity>.Filter.Type(p => p.Id, BsonType.String)))
                .ToListAsync();

            if (profiles.Any())
            {
                foreach (var profile in profiles)
                {
                    await Profiles.InsertOneAsync(profile);
                }

                await Profiles.DeleteManyAsync(Builders<ProfileEntity>.Filter.Not(Builders<ProfileEntity>.Filter.Type(p => p.Id, BsonType.String)));
            }

            var securitySettings = await SecuritySettings
                .Find(Builders<SecuritySettingsEntity>.Filter.Not(Builders<SecuritySettingsEntity>.Filter.Type(p => p.UserId, BsonType.String)))
                .ToListAsync();

            if (securitySettings.Any())
            {
                foreach (var securitySetting in securitySettings)
                {
                    await SecuritySettings.InsertOneAsync(securitySetting);
                }

                await SecuritySettings.DeleteManyAsync(Builders<SecuritySettingsEntity>.Filter.Not(Builders<SecuritySettingsEntity>.Filter.Type(p => p.UserId, BsonType.String)));
            }

            var relations = await FriendRequests
                .Find(Builders<FriendsEntity>.Filter.Not(Builders<FriendsEntity>.Filter.Type(p => p.FromUser, BsonType.String)))
                .ToListAsync();

            if (relations.Any())
            {
                foreach (var relation in relations)
                {
                    await FriendRequests.InsertOneAsync(relation);
                }

                await FriendRequests.DeleteManyAsync(Builders<FriendsEntity>.Filter.Not(Builders<FriendsEntity>.Filter.Type(p => p.FromUser, BsonType.String)));
            }
        }
    }
}
