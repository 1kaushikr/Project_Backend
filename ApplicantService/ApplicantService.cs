using Applicant;
using DataLayer;
using MongoDB.Driver;

namespace ApplicantService
{
    public class Applicantservice:IApplicantService
    {
        private readonly IMongoCollection<Application> _Applicant;
        public Applicantservice()
        {
            var data = new Data();
            _Applicant = data._Applicant;
        }

        public List<Application> Get()
        {
            return _Applicant.Find(Person => true).ToList();
        }


        public Application Post(Application person)
        {
            _Applicant.InsertOne(person);
            return person;
        }
    }
}
