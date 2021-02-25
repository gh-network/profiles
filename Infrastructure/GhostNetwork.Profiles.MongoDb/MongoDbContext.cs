using MongoDB.Driver;

namespace GhostNetwork.Profiles.MongoDb
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase database;

        public MongoDbContext(IMongoDatabase database)
        {
            this.database = database;
        }

        public IMongoCollection<ProfileEntity> Profiles =>
            database.GetCollection<ProfileEntity>("profiles");

        public IMongoCollection<WorkExperienceEntity> WorkExperience =>
            database.GetCollection<WorkExperienceEntity>("workExperience");

        public IMongoCollection<FriendRequestEntity> FriendRequests =>
            database.GetCollection<FriendRequestEntity>("friends");

        public IMongoCollection<SecuritySettingsEntity> SecuritySettings =>
            database.GetCollection<SecuritySettingsEntity>("securitySettings");
    }
}
