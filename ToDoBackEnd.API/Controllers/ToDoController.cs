using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ToDoBackEnd.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ToDoController : ControllerQuery
    {        
        public ToDoController(IConfiguration config, ILogger<ToDoController> logger): base(config, logger) {}

        [HttpGet("{id?}")]
        public async Task<JToken> Get(int? id)
        {            
            JObject payload = null;                        
            if (id.HasValue) {
                payload = new JObject();
                payload["id"] = id.Value;            
            }

            return await this.Query(HttpContext.Request.Method, this.GetType(), payload);
        }

        [HttpDelete("{id}")]
        public async Task<JToken> Delete(int id)
        {           
            var payload = new JObject();
            payload["id"] = id;            

            return await this.Query(HttpContext.Request.Method, this.GetType(), payload);
        }

        [HttpPost]
        public async Task<JToken> Post([FromBody]JToken payload)
        {            
            return await this.Query(HttpContext.Request.Method, this.GetType(), payload);
        }

        [HttpPut("{id}")]
        public async Task<JToken> Put(int id, [FromBody]JToken payload)
        {           
            var wrapper = new JObject();
            wrapper["id"] = id;            
            wrapper["todo"] = payload;

            return await this.Query(HttpContext.Request.Method, this.GetType(), wrapper);
        }
    }
}
