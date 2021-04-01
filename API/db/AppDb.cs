using System;
using System.Text;
using MySql.Data.MySqlClient;

namespace HowTosApi
{
    public class AppDb
    {
        public MySqlConnection Connection {get;}

        public AppDb(string dbConnectionString) {
            this.Connection = new MySqlConnection(dbConnectionString);

        }


/*
        public string GetAll()
        {
            StringBuilder result = new StringBuilder();
            try
            {
                string query = "SELECT * FROM helloworld";
                MySqlCommand command = new MySqlCommand(query, SQL);

                SQL.Open();
                MySqlDataReader data = command.ExecuteReader();

                result.Append("From Database >>> ");

                while (data.Read()) {
                    result.Append(
                        + data.GetInt32(0) + " - "
                        + data.GetString(1) + ", "
                        );
                }

                result.Append(" <<< ");
            }
            catch (Exception e)
            {
                result.Append("--> error" + e.ToString());
            }
            finally
            {
            SQL.Close();
            }

            return result.ToString();
        }
        */
    }
}
