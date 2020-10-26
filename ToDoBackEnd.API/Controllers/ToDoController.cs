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

        [HttpPut("{id}")]
        public async Task<JToken> Put(int id, [FromBody]JToken payload)
        {
            var wrapper = new JObject
            {
                ["id"] = id,
                ["todo"] = payload
            };

            return EnrichResult(await this.Query("put", this.GetType(), wrapper));
        }

        private JToken EnrichResult(JToken source)
        {
            var baseUrl = (HttpContext != null) ? HttpContext.Request.Scheme + "://" + HttpContext.Request.Host : string.Empty;

            var AddUrl = new Action<JObject>(o => 
            {
                var todoUrl = $"{baseUrl}/todo/{o["id"]}";
                o["url"] = todoUrl;
            });

            if (source is JArray)
            {
                foreach(JObject i in (source as JArray))
                {
                    AddUrl(i);
                }
            } else if (source is JObject)
            {
                AddUrl(source as JObject);
            }

            return source;
        }
    }
}
