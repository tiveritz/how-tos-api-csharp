using Microsoft.AspNetCore.Mvc;


namespace HowTosApi.Controllers
{
    [ApiVersion("1")]
    [Route("hwts/v{v:apiVersion}/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private AppDb Db;

        public StatisticsController(AppDb db)
        {
            this.Db = db;
        }

        [HttpGet]
        public IActionResult GetStatistics()
        {
            StatisticsQuery htq = new StatisticsQuery(Db);
            return Ok(htq.GetStatistics());
        }
    }
}
