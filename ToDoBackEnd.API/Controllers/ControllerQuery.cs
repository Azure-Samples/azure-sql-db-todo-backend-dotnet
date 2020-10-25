using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Dapper;

namespace ToDoBackEnd.API.Controllers
{
    public class ControllerQuery: ControllerBase
    {
        private readonly ILogger<ControllerQuery> _logger;
        private readonly IConfiguration _config;

        public ControllerQuery(IConfiguration config, ILogger<ControllerQuery> logger)
        {
            _logger = logger;
            _config = config;
        }

        protected async Task<JToken> Query(string verb, Type entity, JToken payload = null)
        {
            JToken result = null;

            if (!(new string[] {"get", "put", "patch", "delete", "post"}).Contains(verb.ToLower()))
            {
                throw new ArgumentException($"verb '{verb}' not supported", nameof(verb));
            }

            string entityName = entity.Name.Replace("Controller", string.Empty).ToLower();
            string procedure = $"web.{verb}_{entityName}";
            _logger.LogDebug($"Executing {procedure}");

            var connectionStringName = verb.ToLower() != "get" ? "ReadWriteConnection" : "ReadOnlyConnection";
            
            using(var conn = new SqlConnection(_config.GetConnectionString(connectionStringName))) {
                DynamicParameters parameters = new DynamicParameters();

                if (payload != null)
                {
                    parameters.Add("payload", payload.ToString());
                }

                var qr = await conn.ExecuteScalarAsync<string>(
                    sql: procedure, 
                    param: parameters, 
                    commandType: CommandType.StoredProcedure
                );
                
                if (qr != null)
                    result = JToken.Parse(qr);
            };

            if (result == null) 
                result = JToken.Parse("[]");
                        
            return result;
        }
    }
}
