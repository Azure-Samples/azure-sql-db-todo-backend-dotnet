using System;
using DbUp;
using DotNetEnv;

namespace ToDoBackEnd.Deploy
{
    class Program
    {
        static int Main(string[] args)
        {
            DotNetEnv.Env.Load();

            var connectionString = Environment.GetEnvironmentVariable("ConnectionString");
            var backEndUserPassword = Environment.GetEnvironmentVariable("BackEndUserPassword");

            var dbup = DeployChanges.To
                .SqlDatabase(connectionString)
                .WithScriptsFromFileSystem("./sql") 
                .JournalToSqlTable("dbo", "$__dbup_journal")                                               
                .WithVariable("BackEndUserPassword", backEndUserPassword)
                .LogToConsole()
                .Build();

            var result = dbup.PerformUpgrade();

            if (!result.Successful)
            {
                Console.WriteLine(result.Error);
                return -1;
            }

            Console.WriteLine("Success!");
            return 0;
        }
        
    }
}
