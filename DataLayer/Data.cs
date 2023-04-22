using Applicant;
using MongoDB.Driver;


namespace DataLayer
{
    public class Data
    {
        public IMongoCollection<Application> _Applicant;
        public Data()
        {
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            var database = mongoClient.GetDatabase("Applicants");
            _Applicant = database.GetCollection<Application>("Applicant");
        }

    }
}