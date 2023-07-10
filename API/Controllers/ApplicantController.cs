using ApplicantService;
using Applicant;
using Microsoft.AspNetCore.Mvc;
using Query;
using DataLayer;


namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApplicantController : ControllerBase
    {
        private readonly Applicantservice _ApplicantService;
        public ApplicantController()
        {
            _ApplicantService = new Applicantservice();
        }


        // GET: <Controller>
        [HttpGet]
        public ActionResult<List<Application>> Get()
        {
            return _ApplicantService.Get();
        }


        // POST <Controller>
        [HttpPost]
        public void Post(Application value) 
        {
            _ApplicantService.Post(value);
        }

        // GET: <Controller>/id
        [HttpGet("{id:length(24)}")]
        public ActionResult<Application> Get(string id)
        {
            return _ApplicantService.Get(id);
        }

        // POST <Controller>/Query
        [HttpPost("Query")]
        public ActionResult<List<Application>> Query(query query)
        {
            if (query == null)
            {
                throw new Exception("Invalid length of Query");
            }
            return _ApplicantService.Query(query);
        }
    }
}