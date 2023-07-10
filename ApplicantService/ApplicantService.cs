using Applicant;
using DataLayer;
using Query;

namespace ApplicantService
{
    public class Applicantservice
    {
        private readonly DataService _dataService;
        public Applicantservice()
        {
            _dataService = new DataService();
        }
        public List<Application> Get()
        {
            return _dataService.Get();
        }
        public void Post(Application person)
        {
            if (person == null)
            {
                throw new Exception("Data Sent to Server is null");
            }
            _dataService.Post1(person);
            var exp = "";
            if (person.expList != null)
            {
                for (int i = 0; i < person.expList.Count; i++)
                {
                    var v1 = person.expList[i].role.Length > 0 ? " as " + person.expList[i].role : "";
                    var v2 = person.expList[i].org.Length > 0 ? " in " + person.expList[i].org : "";
                    var v3 = person.expList[i].respon.Length > 0 ? " My Responibilities were " + person.expList[i].respon : "";
                    var v4 = v1 + v2 + v3;
                    exp = v4.Length > 0 ? exp + " Worked" + v4 : exp;
                }
            }
            var edu = "";
            if (person.eduList != null)
            {
                for (int i = 0; i < person.eduList.Count; i++)
                {
                    var v1 = person.eduList[i].org.Length > 0 ? " in " + person.eduList[i].org : "";
                    var v2 = person.eduList[i].deg.Length > 0 ? " doing " + person.eduList[i].deg : "";
                    var v3 = person.eduList[i].major.Length > 0 ? " majoring in " + person.eduList[i].major : "";
                    var v4 = v1 + v2 + v3;
                    edu = v4.Length > 0 ? edu + " Studied" + v4 : edu;
                }
            }
            var pro = "";
            if (person.proList != null)
            {
                for (int i = 0; i < person.proList.Count; i++)
                {
                    var v1 = person.proList[i].name.Length > 0 ? " on " + person.proList[i].name : "";
                    var v2 = person.proList[i].respon.Length > 0 ? " My Responibilities were " + person.proList[i].respon : "";
                    var v3 = v1 + v2;
                    pro = v3.Length > 0 ? pro + " Worked" + v3 : pro;
                }
            }
            var skill = " Skilled in "+ person.skill;
            Resume temp = new Resume();
            temp._id = person._id;
            temp.resume = edu+exp+pro+skill;
            _dataService.Post2(temp);
        }
        public Application Get(string id)
        {
            if (id == null)
            {
                throw new Exception("ID Sent to Server is null");
            }
            var arr = _dataService.Get(id);
            if (arr is null)
            {
                throw new Exception("There is not Applicant with this ID");
            }
            return arr;
        }
        public List<Application> Query(query query) 
        {
            if (query == null || query._query.Length == 0)
            {
                throw new Exception("Invalid length of Query");
            }
            var jsonIds = _dataService.fetchIds(query);
            var resumeList = _dataService.fetchApplication(jsonIds);
            return resumeList;
        }
    }
}
