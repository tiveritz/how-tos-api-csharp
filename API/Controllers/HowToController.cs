using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;


namespace HowTosApi.Controllers
{
    [Route("hwts/v1/[controller]")]
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
            HowToQuery htq = new HowToQuery(Db);
            return Ok(htq.GetAll());
        }

        [Route("{id}")] //hwts/v1/howtos/2
        public IActionResult GetHowToById(int id)
        {
            HowToQuery htq = new HowToQuery(Db);
            return Ok(htq.GetOne(id));
        }
    }
}
