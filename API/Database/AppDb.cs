using System;
using MySql.Data.MySqlClient;


namespace HowTosApi
{
    public class AppDb
    {
        public MySqlConnection Connection {get;}

        public AppDb(string dbConnectionString) {
            this.Connection = new MySqlConnection(dbConnectionString);
        }

        internal int ExecuteNoneQueryAndGetId(MySqlCommand cmd)
        {
            int id = -1;
            try
            {
                this.Connection.Open();
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                id = (Int32)cmd.LastInsertedId;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
            this.Connection.Close();
            }
            return id;
        }

        internal void ExecuteNonQuery(MySqlCommand cmd)
        {
            try
            {
                this.Connection.Open();
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
            this.Connection.Close();
            }
        }
    }
}
