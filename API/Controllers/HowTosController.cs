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

        [Route("{id}")]
        [HttpGet]
        public IActionResult GetHowToById(string id)
        {
            HowToQuery htq = new HowToQuery(Db);
            return Ok(htq.GetHowToById(id));
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

        [HttpPost]
        public IActionResult CreateHowTo([FromBody]CreateHowTo createHowTo)
        {
            HowTosQuery htq = new HowTosQuery(Db);
            string uriId = htq.CreateHowTo(createHowTo);

            HowToQuery newHtq = new HowToQuery(Db);
            HowTo newHowTo = newHtq.GetHowToById(uriId);

            return CreatedAtAction(nameof(GetHowToById), new { id = uriId }, newHowTo);
        }
    }
}
