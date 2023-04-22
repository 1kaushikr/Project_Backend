using Applicant;

namespace ApplicantService
{
    public interface IApplicantService
    {
        List<Application> Get();
        Application Post(Application person);
    }
}
