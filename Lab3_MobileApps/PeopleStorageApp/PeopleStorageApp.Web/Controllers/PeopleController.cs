using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PeopleStorageApp.DataContracts;
using PeopleStorageApp.Web.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeopleStorageApp.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly LocalDataStorage data;

        public PeopleController(LocalDataStorage data)
        {
            this.data = data;
        }

        [HttpGet]
        public IActionResult GetPeople()
        {
            return Ok(data.People);
        }

        [HttpPost]
        public IActionResult AddPerson([FromBody] Person person)
        {
            data.AddPerson(person);
            return Ok();
        }

        [HttpGet("{id}/photo")]
        public IActionResult GetPhoto([FromRoute] int id)
        {
            var p = data.People.First(w => w.Id == id);
            return base.File(Convert.FromBase64String(p.PictureBase64), "image/jpeg");
        }
    }
}
