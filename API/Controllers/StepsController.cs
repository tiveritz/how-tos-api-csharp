using Microsoft.AspNetCore.Mvc;


namespace HowTosApi.Controllers
{
    [ApiVersion("1")]
    [Route("hwts/v{v:apiVersion}/[controller]")]
    [ApiController]
    public class StepsController : ControllerBase
    {
        private AppDb Db;

        public StepsController(AppDb db)
        {
            this.Db = db;
        }

        [HttpGet]
        public IActionResult GetAllSteps()
        {
            StepsQuery htq = new StepsQuery(Db);
            return Ok(htq.GetAll());
        }
        
        [HttpPost]
        public IActionResult CreateStep([FromBody]CreateStep createStep)
        {
            StepsQuery sq = new StepsQuery(Db);
            string uriId = sq.CreateStep(createStep);

            StepQuery newSq = new StepQuery(Db);
            Step newStep = newSq.GetStepById(uriId);

            return CreatedAtAction(nameof(GetStepById), new { id = uriId }, newStep);
        }

        [Route("{id}")]
        [HttpGet]
        public IActionResult GetStepById(string id)
        {
            StepQuery sq = new StepQuery(Db);
            Step s = sq.GetStepById(id);
            
            if (s is not null)
            {
                return Ok(s);
            }
            return NotFound();
        }

        [Route("{id}")]
        [HttpPatch]
        public IActionResult ChangeStep(string id, [FromBody]ChangeStep changeStep)
        {
            StepQuery sq = new StepQuery(Db);
            Step s = sq.GetStepById(id);
            
            if (s is not null)
            {
                sq.ChangeStep(id, changeStep);
                return Ok(s);
            }
            return NotFound();


        }

        [Route("{id}")]
        [HttpDelete]
        public IActionResult DeleteStepById(string id)
        {
            StepQuery sq = new StepQuery(Db);

            if (sq.StepExists(id))
            {
                sq.DeleteStep(id);
                return NoContent();
            }
            return NotFound();
        }

        [Route("{id}/steps")]
        [HttpPost]
        public IActionResult LinkStep(string id, [FromBody]LinkStep linkStep)
        {
            SubstepQuery sq = new SubstepQuery(Db);
            sq.linkStepToSuper(id, linkStep.Id);

            return Accepted();
        }
    }
}
