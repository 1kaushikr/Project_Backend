using MongoDB.Bson.Serialization.Attributes;

namespace Applicant
{
    [BsonIgnoreExtraElements]
    public class Application
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? _id { get; set; }
        public string? firstName { get; set; }

        public string? lastName { get; set; }

        public string? dob { get; set; }
        public List<string>? phoneList { get; set; }
        public List<string>? emailList { get; set; }
        public List<Edu>? eduList { get; set; }
        public List<Exp>? expList { get; set; }
        public List<Pro>? proList { get; set; }
        public string? skill { get; set; }

    }
}