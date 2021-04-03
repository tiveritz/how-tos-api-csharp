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

        [HttpGet]//hwts/v1/howtos
        public IActionResult GetAllHowTos()
        {
            HowTosQuery htq = new HowTosQuery(Db);
            return Ok(htq.GetAll());
        }

        [Route("{id}")] //hwts/v1/howtos/a9d8cd7a
        [HttpGet]
        public IActionResult GetHowToById(string id)
        {
            HowToQuery htq = new HowToQuery(Db);
            return Ok(htq.GetOne(id));
        }
    }
}
