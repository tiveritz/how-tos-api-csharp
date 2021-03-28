using System;

namespace how_tos_api
{
    public class HelloWorld
    {
        public string helloWorld {get; set;}

        public HelloWorld() {
            string dbConnectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            MySqlConnector db = new MySqlConnector(dbConnectionString);

            this.helloWorld = db.GetAll();;
        }
    }
}
