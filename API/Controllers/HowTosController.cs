using System;
using Microsoft.AspNetCore.Mvc;


namespace HowTosApi.Controllers
{
    [ApiVersion("1")]
    [Route("hwts/v{v:apiVersion}/[controller]")]
    [ApiController]
    public class HowTosController : ControllerBase
    {
        private AppDb Db;

        public HowTosController(AppDb db)
        {
            this.Db = db;
        }

        [HttpGet]
        public IActionResult GetAllHowTos()
        {
            HowTosQuery htq = new HowTosQuery(Db);
            return Ok(htq.GetAll());
        }

        [HttpPost]
        public IActionResult CreateHowTo([FromBody]CreateHowTo createHowTo)
        {
            HowTosQuery htq = new HowTosQuery(Db);
            string uriId = htq.CreateHowTo(createHowTo);

            HowToQuery newHtq = new HowToQuery(Db);
            HowTo newHowTo = newHtq.GetHowToById(uriId);

            return CreatedAtAction(nameof(GetHowToById), new { id = uriId }, newHowTo);
        }

        [Route("{id}")]
        [HttpGet]
        public IActionResult GetHowToById(string id)
        {
            HowToQuery htq = new HowToQuery(Db);
            HowTo ht = htq.GetHowToById(id);

            if (ht == null)
            {
                return NotFound();
            }

            return Ok(ht);
        }

        [Route("{id}")]
        [HttpPatch]
        public IActionResult ChangeHowTo(string id, [FromBody]ChangeHowTo changeHowTo)
        {
            HowToQuery htq = new HowToQuery(Db);
            HowTo ht = htq.GetHowToById(id);
            
            if (ht == null)
            {
                return NotFound();
            }

            htq.ChangeHowTo(id, changeHowTo);

            return Ok();
        }
    
        [Route("{id}")]
        [HttpDelete]
        public IActionResult DeleteHowToById(string id)
        {
            HowToQuery htq = new HowToQuery(Db);
            HowTo ht = htq.GetHowToById(id);
            
            if (ht == null)
            {
                return NotFound();
            }

            htq.DeleteHowTo(id);

            return NoContent();
        }

        [Route("{id}/steps")]
        [HttpPost]
        public IActionResult LinkStep(string id, [FromBody]LinkStep linkStep)
        {
            SubstepQuery sq = new SubstepQuery(Db);
            sq.linkStepToHowTo(id, linkStep.Id);
            
            return Accepted();
        }

        [Route("{id}/steps")]
        [HttpPatch]
        public IActionResult ChangeStepsOrder(string id, [FromBody]ChangeOrder changeOrder)
        {


            return Ok();
        }

        [Route("{id}/steps")]
        [HttpDelete]
        public IActionResult DeleteLinkedStep(string id, [FromBody]LinkStep linkStep)
        {


            return Ok();
        }
    }
}
