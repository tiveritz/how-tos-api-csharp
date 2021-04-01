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
            cmd.CommandText = @"
                SELECT HowTosUriIds.uri_id, HowTos.title, HowTos.ts_create, HowTos.ts_update
                FROM HowTos
                JOIN HowTosUriIds ON HowTos.id=HowTosUriIds.how_to_id;";

            List<HowTo> howTos = QueryRead(cmd);
 
            return howTos;
        }
        public HowTo GetOne(string uriId)
        {
            MySqlCommand cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"
                SELECT HowTosUriIds.uri_id, HowTos.title, HowTos.ts_create, HowTos.ts_update
                FROM HowTos
                JOIN HowTosUriIds ON HowTos.id=HowTosUriIds.how_to_id
                WHERE uri_id=@uriId";
            cmd.Parameters.AddWithValue("@uriId", "a9d8cd7a");

            List<HowTo> howTos = QueryRead(cmd);
            
            return howTos.Count > 0 ? howTos[0] : null;
        }

        private List<HowTo> QueryRead(MySqlCommand cmd)
        {
            List<HowTo> howTos = new List<HowTo>();

            try
            {
                Db.Connection.Open();
                cmd.Prepare();
                MySqlDataReader data = cmd.ExecuteReader();

                while (data.Read()) {
                    string uriId = data.GetString(0);
                    HowTo howTo = new HowTo()
                        {
                        Id = uriId,
                        Title = data.GetString(1),
                        Created = data.GetDateTime(2),
                        Updated = data.GetDateTime(3)
                        };
                    howTo.CreateLink(uriId);
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
