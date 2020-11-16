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
                payload = new JObject
                {
                    ["id"] = id.Value
                };
            }

            return EnrichResult(await this.Query("get", this.GetType(), payload));
        }

        [HttpDelete("{id?}")]
        public async Task<JToken> Delete(int? id)
        { 
            JObject payload = null;                        
            if (id.HasValue) {
                payload = new JObject
                {
                    ["id"] = id.Value
                };
            }

            return EnrichResult(await this.Query("delete", this.GetType(), payload));
        }

        [HttpPost]
        public async Task<JToken> Post([FromBody]JToken payload)
        {            
            return EnrichResult(await this.Query("post", this.GetType(), payload));
        }

        [HttpPatch("{id}")]
        public async Task<JToken> Patch(int id, [FromBody]JToken payload)
        {
            var wrapper = new JObject
            {
                ["id"] = id,
                ["todo"] = payload
            };

            return EnrichResult(await this.Query("patch", this.GetType(), wrapper));
        }

        private JToken EnrichResult(JToken source)
        {
            var baseUrl = (HttpContext != null) ? HttpContext.Request.Scheme + "://" + HttpContext.Request.Host : string.Empty;

            var AddUrl = new Action<JObject>(o => 
            {
                if (o == null) return;
                var todoUrl = $"{baseUrl}/todo/{o["id"]}";
                o["url"] = todoUrl;
            });

            (source as JArray)?.ToList().ForEach(e => AddUrl(e as JObject));
            AddUrl(source as JObject);

            return source;
        }
    }
}
