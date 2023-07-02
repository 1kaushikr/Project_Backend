using Applicant;
using DataLayer;
using ElasticResume;
using Nest;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;

namespace ApplicantService
{
    public class Applicantservice : IApplicantService
    {
        private readonly IMongoCollection<Application> _mongoCollection;
        private readonly ElasticClient _ElasticClient;
        public Applicantservice()
        {
            var data = new Data();
            _mongoCollection = data._mongoCollection;
            _ElasticClient = data._ElasticClient;
        }

        public List<Application> Get()
        {
            return _mongoCollection.Find(Person => true).ToList();
        }


        public Application Post(Application person)
        {
            _mongoCollection.InsertOne(person);
            var exp = "";
            if (person.expList != null)
            {
                for (int i = 0; i < person.expList.Count; i++)
                {
                    var v1 = person.expList[i].role.Length > 0 ? " as " + person.expList[i].role : "";
                    var v2 = person.expList[i].org.Length > 0 ? " in " + person.expList[i].org : "";
                    var v3 = person.expList[i].startDate.Length > 0 ? " from " + person.expList[i].startDate : "";
                    var v4 = person.expList[i].endDate.Length > 0 ? " to " + person.expList[i].endDate : "";
                    var v5 = person.expList[i].respon.Length > 0 ? " My Responibilities were " + person.expList[i].respon : "";
                    var v6 = v1 + v2 + v3 + v4 + v5;
                    exp = v6.Length > 0 ? exp + " Worked" + v6 : exp;
                }
            }
            var edu = "";
            if (person.eduList != null)
            {
                for (int i = 0; i < person.eduList.Count; i++)
                {
                    var v1 = person.eduList[i].org.Length > 0 ? " in " + person.eduList[i].org : "";
                    var v2 = person.eduList[i].startDate.Length > 0 ? " from " + person.eduList[i].startDate : "";
                    var v3 = person.eduList[i].endDate.Length > 0 ? " to " + person.eduList[i].endDate : "";
                    var v4 = person.eduList[i].deg.Length > 0 ? " doing " + person.eduList[i].deg : "";
                    var v5 = person.eduList[i].major.Length > 0 ? " majoring in " + person.eduList[i].major : "";
                    var v6 = v1 + v2 + v3 + v4 + v5;
                    edu = v6.Length > 0 ? edu + " Studied" + v6 : edu;
                }
            }
            var pro = "";
            if (person.proList != null)
            {
                for (int i = 0; i < person.proList.Count; i++)
                {
                    var v1 = person.proList[i].name.Length > 0 ? " on " + person.proList[i].name : "";
                    var v2 = person.proList[i].startDate.Length > 0 ? " from " + person.proList[i].startDate : "";
                    var v3 = person.proList[i].endDate.Length > 0 ? " to " + person.proList[i].endDate : "";
                    var v4 = person.proList[i].respon.Length > 0 ? " My Responibilities were " + person.proList[i].respon : "";
                    var v5 = v1 + v2 + v3 + v3 + v4;
                    pro = v5.Length > 0 ? pro + "Worked" + v5 : pro;
                }
            }
            var skill = "";
            if (person.skill != null)
            {
                for (int i = 0; i < person.skill.Count; i++)
                {
                    skill = skill + person.skill[i] +" ";
                }
            }
            elasticResume temp = new elasticResume();
            temp.id = person._id;
            temp.edu = edu;
            temp.exp = exp;
            temp.skill = skill;
            temp.pro = pro;

            var indexResponse = _ElasticClient.IndexDocument(temp);

            if (indexResponse.IsValid)
            {
                Console.WriteLine($"Index document with ID {indexResponse.Id} succeeded.");
            }
            return person;
        }

        public Application? Get(string id)
        {
            if (id == null)
            {
                return null;
            }
            var arr = _mongoCollection.Find(x => x._id == id).FirstOrDefault();

            if (arr is null)
            {
                return null;
            }
            return arr;
        }
        public List<Application> Query(string s)
        {
            JObject json = JObject.Parse(s);
            var searchResponse = _ElasticClient.Search<elasticResume>(s => s.Query(q => q
                                                                                .Term(p => p.skill, json["SKILL"].ToString()) || q
                                                                                .Term(p => p.exp, json["SKILL"].ToString()) || q
                                                                                .Term(p => p.exp, json["ORG"].ToString()) || q
                                                                                .Term(p => p.edu, json["ORG"].ToString()) || q
                                                                                .Term(p => p.edu, json["SKILL"].ToString()) || q
                                                                                .Term(p => p.pro, json["SKILL"].ToString()) || q
                                                                                ));
            
            var resumeList = new List<Application>();
            var resumes = searchResponse.Documents.ToArray();
            HashSet<string> Ids = new HashSet<string>();
            foreach ( var resume in resumes )
                    if (!Ids.Contains(resume.id))
                {
                    resumeList.Add(this.Get(resume.id));
                }

            return resumeList; 
        }
    }
}
