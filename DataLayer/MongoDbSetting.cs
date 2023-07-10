
using MongoDB.Driver;

namespace DataLayer
{
    public class MongoDbSetting
    {
        public string ConnectionString { get; set; } = "mongodb://localhost:27017";

        public string DatabaseName { get; set; } = "Applicants";
    }
}