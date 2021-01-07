using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace GhostNetwork.Profiles.MongoDb
{
    public class WorkExperienceEntity
    {
        public ObjectId Id { get; set; }

        [BsonElement("companyName")]
        public string CompanyName { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("startWork")]
        public long? StartWork { get; set; }

        [BsonElement("finishWork")]
        public long? FinishWork { get; set; }

        [BsonElement("profileId")]
        public string ProfileId { get; set; }
    }
}
