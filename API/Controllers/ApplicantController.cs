using ApplicantService;
using Applicant;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApplicantController : ControllerBase
    {
        private readonly IApplicantService _ApplicantService;
        public ApplicantController()
        {
            this._ApplicantService = new Applicantservice();
        }

        // GET: <Controller>
        [HttpGet]
        public ActionResult<List<Application>> Get()
        {
            return _ApplicantService.Get();
        }


        // POST <Controller>
        [HttpPost]
        public ActionResult<Application> Post([FromBody] Application value)
        {
            _ApplicantService.Post(value);
            return CreatedAtAction(nameof(Get), new { id = value._id }, value);
        }

    }
}