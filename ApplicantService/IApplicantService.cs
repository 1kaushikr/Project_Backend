using Applicant;
using MongoDB.Bson;

namespace ApplicantService
{
    public interface IApplicantService
    {
        List<Application> Get();
        Application Post(Application person);
        Application? Get(string id);
        List<Application> Query(string s);
    }
}
