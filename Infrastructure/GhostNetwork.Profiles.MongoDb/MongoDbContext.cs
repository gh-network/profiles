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

        public IMongoCollection<SecuritySettingsEntity> SecuritySettings =>
            database.GetCollection<SecuritySettingsEntity>("securitySettings");

        public IMongoCollection<FriendsEntity> Sent =>
            database.GetCollection<FriendsEntity>("friendsSentRequests");
        
        public IMongoCollection<FriendsEntity> Received =>
            database.GetCollection<FriendsEntity>("friendsReceivedRequests");
    }
}
