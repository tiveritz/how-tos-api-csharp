using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;


namespace HowTosApi.Controllers
{
    public class HowTosQuery
    {
        private AppDb Db;
        private string GetAllQuery = @"
            SELECT HowTosUriIds.uri_id, HowTos.title, HowTos.ts_create, HowTos.ts_update
            FROM HowTos
            JOIN HowTosUriIds ON HowTos.id=HowTosUriIds.how_to_id;";
        private string GetOneQuery = @"
            SELECT HowTosUriIds.uri_id, HowTos.title, HowTos.ts_create, HowTos.ts_update
            FROM HowTos
            JOIN HowTosUriIds ON HowTos.id=HowTosUriIds.how_to_id
            WHERE uri_id=@uriId";

        public HowTosQuery(AppDb db)
        {
            Db = db;
        }

        public List<HowTos> GetAll()
        {
            MySqlCommand cmd = Db.Connection.CreateCommand();
            cmd.CommandText = GetAllQuery;

            List<HowTos> howTos = QueryRead(cmd);
 
            return howTos;
        }

        public HowTos GetOne(string uriId)
        {
            MySqlCommand cmd = Db.Connection.CreateCommand();
            cmd.CommandText = GetOneQuery;
            cmd.Parameters.AddWithValue("@uriId", uriId);

            List<HowTos> howTos = QueryRead(cmd);
            
            return howTos.Count > 0 ? howTos[0] : null;
        }

        private List<HowTos> QueryRead(MySqlCommand cmd)
        {
            List<HowTos> howTos = new List<HowTos>();

            try
            {
                Db.Connection.Open();
                cmd.Prepare();
                MySqlDataReader data = cmd.ExecuteReader();

                while (data.Read()) {
                    HowTos howTo = new HowTos()
                        {
                        Title = data.GetString(1),
                        Created = data.GetDateTime(2),
                        Updated = data.GetDateTime(3)
                        };
                    howTo.SetId(data.GetString(0));
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
