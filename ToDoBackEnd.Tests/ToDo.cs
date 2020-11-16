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
using System.Linq;

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

            // Target correct database based on branch name
            var csb = new SqlConnectionStringBuilder(connectionString);
            var branchName = Environment.GetEnvironmentVariable("GITHUB_REF");
            branchName = branchName.Replace("refs/heads/", string.Empty);
            branchName = branchName == "main" ? string.Empty : "_" + branchName;
            csb.InitialCatalog += branchName;
            Console.WriteLine($"Testing database: {csb.InitialCatalog}");

            var inMemoryConfiguration = new Dictionary<string, string>()
            {
                { "ConnectionStrings:ReadWriteConnection", csb.ConnectionString },
                { "ConnectionStrings:ReadOnlyConnection", csb.ConnectionString }
            };
            var config = new ConfigurationBuilder().AddInMemoryCollection(inMemoryConfiguration).Build();

            // Create controller
            _controller = new ToDoController(config, logger);        
        }

        [SetUp]
        public void Setup()
        {
        }

        [Test, Order(100)]
        public async Task Respond_To_GET()
        {
            var result = await _controller.Get(null);

            Assert.IsInstanceOf(typeof(JArray), result);            
        }

        [Test, Order(200)]
        public async Task Respond_To_POST()
        {
            JObject payload = new JObject { ["title"] = "a todo" };

            var result = await _controller.Post(payload);

            Assert.IsInstanceOf(typeof(JObject), result);
            Assert.AreEqual("a todo", result["title"].Value<string>());
        }

        [Test, Order(300)]
        public async Task Respond_To_DELETE()
        {
            var result = await _controller.Delete(null);

            Assert.IsInstanceOf(typeof(JArray), result);
            Assert.IsEmpty(result);
        }

        [Test, Order(400)]
        public async Task GET_After_DELETE_All_Is_Empty()
        {
            await _controller.Delete(null);
            var result = await _controller.Get(null);

            Assert.IsInstanceOf(typeof(JArray), result);
            Assert.IsEmpty(result);
        }

        [Test, Order(500)]
        public async Task POST_has_Url()
        {
            JObject payload = new JObject { ["title"] = "walk the dog" };

            await _controller.Post(payload);
            var result = await _controller.Get(null);

            Assert.IsInstanceOf(typeof(JArray), result);
            Assert.That(result.ToList().Count == 1);
            Assert.AreEqual("walk the dog", result[0]["title"].Value<string>());
        }

        [Test, Order(600)]
        public async Task New_Todo_Is_Not_Complete()
        {
            JObject payload = new JObject { ["title"] = "a new todo" };
            
            var result = await _controller.Post(payload);

            Assert.IsInstanceOf(typeof(JObject), result);            
            Assert.AreEqual(false, result["completed"].Value<bool>());
        }

        [Test, Order(700)]
        public async Task Each_Todo_Has_A_Url()
        {
            JObject payload = new JObject { ["title"] = "another todo" };

            await _controller.Post(payload);
            var result = await _controller.Get(null);

            Assert.IsInstanceOf(typeof(JArray), result);
            Assert.That(result.All(i => i["url"].Type == JTokenType.String));
        }

        //[Test, Order(800)]
        //public async Task New_Todo_With_Order()
        //{
        //    JObject payload = new JObject { ["title"] = "blah" };

        //    var result = await _controller.Post(payload);

        //    Assert.IsInstanceOf(typeof(JObject), result);
        //    Assert.IsNotNull(result["order"]);
        //}
    }
}