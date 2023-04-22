using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ApplicantLogic
{
    [BsonIgnoreExtraElements]
    public class Applicant
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        [BsonElement("firstName")]
        public string FirstName { get; set; }

        [BsonElement("lastName")]
        public string LastName { get; set; }

        [BsonElement("dob")]
        public DateTime DateOfBirth { get; set; }

        [BsonElement("phones")]
        public List<string> Phones { get; set; }
        [BsonElement("emails")]
        public List<string> EmailsList { get; set; }
        [BsonElement("educations")]
        public List<Education> Educations { get; set; }
        [BsonElement("workExperience")]
        public List<WorkExperience> WorkExperience { get; set; }
        [BsonElement("project")]
        public List<Project> Project { get; set; }
        [BsonElement("skills")]
        public List<string> Skills { get; set; }

    }
}