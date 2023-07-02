using Applicant;
using Nest;
using MongoDB.Driver;


namespace DataLayer
{
    public class Data
    {
        public IMongoCollection<Application> _mongoCollection;
        public ElasticClient _ElasticClient;
        public Data()
        {
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            var database = mongoClient.GetDatabase("Applicants");
            _mongoCollection = database.GetCollection<Application>("Applicant");
            var settings = new ConnectionSettings(new Uri("http://localhost:9200")).DefaultIndex("applicant");
            this._ElasticClient = new ElasticClient(settings);
        }

    }
}