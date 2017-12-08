using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyWebApiApp.Models;
using System.Net;

namespace MyWebApiApp.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly IValuesRepository valuesRepository;
        public ValuesController(IValuesRepository valuesRepository)
        {
            this.valuesRepository = valuesRepository;
        }
        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<ValueDto>> Get()
        {
            return (await valuesRepository
                .GetAll())
                .Select(v => new ValueDto { Id = v.Id, Value = v.Value } );
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<string> Get(int id)
        {
            return (await valuesRepository.GetAll()).Single(v => v.Id == id).Value;
        }

        // POST api/values
        [HttpPost]
        public async Task Post([FromBody]string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                HttpContext.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return;
            }
            await valuesRepository.Create(value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody]string value)
        {
            await valuesRepository.Update(id, value);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await valuesRepository.Remove(id);
        }
    }
}
