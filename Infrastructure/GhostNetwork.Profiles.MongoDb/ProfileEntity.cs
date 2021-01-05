using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace GhostNetwork.Profiles.MongoDb
{
    public class ProfileEntity
    {
        public ObjectId Id { get; set; }

        [BsonElement("firstName")]
        public string FirstName { get; set; }

        [BsonElement("lastName")]
        public string LastName { get; set; }

        [BsonElement("gender")]
        public string Gender { get; set; }

        [BsonElement("dateOfBirth")]
        public long? DateOfBirth { get; set; }

        [BsonElement("city")]
        public string City { get; set; }
    }
}
