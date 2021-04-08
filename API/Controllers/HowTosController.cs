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

        [HttpGet]//hwts/v1/HowTos
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

        [Route("{id}")] //hwts/v1/HowTos/a9d8cd7a
        [HttpGet]
        public IActionResult GetHowToById(string id)
        {
            HowToQuery htq = new HowToQuery(Db);
            return Ok(htq.GetHowToById(id));
        }
    }
}
