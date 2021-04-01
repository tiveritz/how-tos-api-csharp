using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Mvc;


namespace HowTosApi.Controllers
{
    public class HowToQuery
    {
        private AppDb Db;

        public HowToQuery(AppDb db)
        {
            Db = db;
        }

        public List<HowTo> GetAll()
        {
            MySqlCommand cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "@SELECT title, ts_create, ts_update FROM HowTos;";

            List<HowTo> howTos = QueryRead(cmd);
 
            return howTos;
        }
        public HowTo GetOne(int howToId)
        {
            MySqlCommand cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM HowTos WHERE id=@id";
            cmd.Parameters.AddWithValue("@id", howToId);

            List<HowTo> howTos = QueryRead(cmd);

            return howTos.Count > 0 ? howTos[0] : null;
        }

        private List<HowTo> QueryRead(MySqlCommand cmd)
        {
            List<HowTo> howTos = new List<HowTo>();

            try
            {
                Db.Connection.Open();
                MySqlDataReader data = cmd.ExecuteReader();

                while (data.Read()) {
                    int id = data.GetInt32(0);
                    HowTo howTo = new HowTo()
                        {
                        Id = id,
                        Title = data.GetString(1),
                        Created = data.GetDateTime(2),
                        Updated = data.GetDateTime(3)
                        };
                    howTo.CreateLink(id);
                    howTos.Add(howTo);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
            finally
            {
            Db.Connection.Close();
            }

            return howTos;
        }
    }
}
