using MySql.Data.MySqlClient;


namespace HowTosApi
{
    public class AppDb
    {
        public MySqlConnection Connection {get;}

        public AppDb(string dbConnectionString) {
            this.Connection = new MySqlConnection(dbConnectionString);
        }
    }
}
