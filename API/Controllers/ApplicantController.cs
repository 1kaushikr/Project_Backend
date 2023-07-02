using ApplicantService;
using Applicant;
using Microsoft.AspNetCore.Mvc;
using Query;
using System.Net.Http.Headers;


namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApplicantController : ControllerBase
    {
        private readonly IApplicantService _ApplicantService;
        private readonly HttpClient client;
        public ApplicantController()
        {
            this._ApplicantService = new Applicantservice();
            this.client = new HttpClient();
            this.client.BaseAddress = new Uri("http://localhost:5000/");
            this.client.DefaultRequestHeaders.Accept.Clear();
            this.client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }


        // GET: <Controller>
        [HttpGet]
        public ActionResult<List<Application>> Get()
        {
            return _ApplicantService.Get();
        }


        // POST <Controller>
        [HttpPost]
        public ActionResult<Application> Post(Application value) 
        {
            _ApplicantService.Post(value);
            return CreatedAtAction(nameof(Get), new { id = value._id }, value);
        }
        [HttpGet("{id:length(24)}")]
        public ActionResult<Application> Get(string id)
        {
            return  _ApplicantService.Get(id);
        }

        [HttpPost("Query")]
        public async Task<ActionResult<List<Application>>> Query(query query)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("query", query);
            string s = "";
            if (response.IsSuccessStatusCode)
            {
                s =await response.Content.ReadAsStringAsync();
            }
            return _ApplicantService.Query(s);
        }
    }
}