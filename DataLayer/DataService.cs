using MongoDB.Driver;
using Applicant;
using Query;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Newtonsoft.Json.Linq;
using Nest;

namespace DataLayer
{
    public class DataService
    {
        private readonly IMongoCollection<Application> _mongoCollection1;
        private readonly IMongoCollection<Resume> _mongoCollection2;
        private readonly HttpClient _pythonClient;

        public DataService()
        {
            MongoDbSetting mongoDbSetting = new MongoDbSetting();
            PythonAPI pythonAPI = new PythonAPI();
            var mongoClient = new MongoClient(mongoDbSetting.ConnectionString);
            var database = mongoClient.GetDatabase(mongoDbSetting.DatabaseName);
            _mongoCollection1 = database.GetCollection<Application>("Application");
            _mongoCollection2 = database.GetCollection<Resume>("Resume");
            _pythonClient = new HttpClient();
            _pythonClient.BaseAddress = new Uri(pythonAPI.BaseAddress);
            _pythonClient.DefaultRequestHeaders.Accept.Clear();
            _pythonClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public List<Application> Get()
        {
            try
            {
                return _mongoCollection1.Find(_ => true).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to get data from to MongoDB due to" + ex.Message);
            }
        }
        public void Post1(Application person)
        {
            try
            {
                _mongoCollection1.InsertOne(person);
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to post data to MongoDB due to" + ex.Message);
            }
        }
        public void Post2(Resume person)
        {
            try
            {
                _mongoCollection2.InsertOne(person);
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to post data to MongoDB due to" + ex.Message);
            }
        }
        public Application Get(string id)
        {
            try
            {
                return _mongoCollection1.Find(x => x._id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to post data to MongoDB due to" + ex.Message);
            }
        }
        public JObject fetchIds(query query)
        {
            HttpResponseMessage response =  _pythonClient.PostAsJsonAsync("query", query).Result;
            var Ids = response.Content.ReadAsStringAsync().Result;
            JObject jsonIds = JObject.Parse(Ids);
            return jsonIds;
        }
        public List<Application> fetchApplication(JObject jsonIds)
        {
            List<Application> resumeList = new List<Application>();
            var idsAray = jsonIds["ids"].ToArray();
            foreach (var item in idsAray)
            {
                resumeList.Add(Get(item.ToString()));
            }
            return resumeList;
        }
    }
}
