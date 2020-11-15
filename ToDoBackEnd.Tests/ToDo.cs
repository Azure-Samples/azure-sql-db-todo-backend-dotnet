using System;
using NUnit.Framework;
using DotNetEnv;
using Microsoft.Data.SqlClient;
using Dapper;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Configuration;
using ToDoBackEnd.API;
using ToDoBackEnd.API.Controllers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToDoBackEnd.Tests
{
    public class Tests
    {
        private ToDoController _controller;

        [OneTimeSetUp]
        public void Initialize()
        {
            // Access to .env file used for local development
            DotNetEnv.Env.Load(Environment.CurrentDirectory + "/../../../" + Env.DEFAULT_ENVFILENAME);
           
            // Create logger
            var loggerFactory = LoggerFactory.Create(b => b.AddConsole());
            var logger = loggerFactory.CreateLogger<ToDoController>();

            // Create In-Memory configuration
            var connectionString = Environment.GetEnvironmentVariable("ConnectionString");
            var inMemoryConfiguration = new Dictionary<string, string>()
            {
                { "ConnectionStrings:ReadWriteConnection", connectionString },
                { "ConnectionStrings:ReadOnlyConnection", connectionString }
            };
            var config = new ConfigurationBuilder().AddInMemoryCollection(inMemoryConfiguration).Build();

            // Create controller
            _controller = new ToDoController(config, logger);        
        }

        [SetUp]
        public void Setup()
        {
        }

        [Test, Order(1)]
        public async Task Respond_To_POST()
        {
            JObject payload = new JObject
            {
                ["title"] = "a todo"
            };

            var result = await _controller.Post(payload);

            Assert.IsInstanceOf(typeof(JObject), result);
            Assert.AreEqual("a todo", (string)((JObject)result)["title"]);
        }

        [Test, Order(2)]
        public async Task Respond_To_DELETE()
        {
            var result = await _controller.Delete(null);

            Assert.IsInstanceOf(typeof(JArray), result);
            Assert.IsEmpty(result);
        }

        [Test, Order(3)]
        public async Task GET_After_DELETE_All_Is_Empty()
        {
            await _controller.Delete(null);
            var result = await _controller.Get(null);

            Assert.IsInstanceOf(typeof(JArray), result);
            Assert.IsEmpty(result);
        }

        [Test, Order(4)]
        public async Task POST_has_Url()
        {
            JObject payload = new JObject
            {
                ["title"] = "walk the dog"
            };

            var result = await _controller.Post(payload);

            Assert.IsInstanceOf(typeof(JObject), result);
            Assert.IsNotEmpty((string)((JObject)result)["url"]);
        }
    }
}