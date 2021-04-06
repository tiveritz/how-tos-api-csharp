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
        private string CreateQuery = @"
            INSERT INTO HowTos (title)
            VALUES (@title)";
        private string MapUriId = @"
            INSERT INTO HowTosUriIds (how_to_id, uri_id)
            VALUES (@id, @uriId)";

        public HowTosQuery(AppDb db)
        {
            Db = db;
        }

        public List<HowToListItem> GetAll()
        {
            MySqlCommand cmd = Db.Connection.CreateCommand();
            cmd.CommandText = GetAllQuery;

            List<HowToListItem> howTos = QueryRead(cmd);
 
            return howTos;
        }

        public string CreateHowTo(CreateHowTo createHowTo)
        {
            // Writes new HowTo to db
            MySqlCommand createHowToCmd = Db.Connection.CreateCommand();
            createHowToCmd.CommandText = CreateQuery;
            createHowToCmd.Parameters.AddWithValue("@title", createHowTo.Title);

            int id = Db.ExecuteNoneQueryAndGetId(createHowToCmd);

            // Generates a uriId
            string uriId = UriIdGenerator.Generate(id);

            // Map id to uriId
            MySqlCommand createUidCmd = Db.Connection.CreateCommand();
            createUidCmd.CommandText = MapUriId;
            createUidCmd.Parameters.AddWithValue("@id", id);
            createUidCmd.Parameters.AddWithValue("@uriId", uriId);

            Db.ExecuteNonQuery(createUidCmd);
            
            return uriId;
        }

        private List<HowToListItem> QueryRead(MySqlCommand cmd)
        {
            List<HowToListItem> howTos = new List<HowToListItem>();

            try
            {
                Db.Connection.Open();
                cmd.Prepare();
                MySqlDataReader data = cmd.ExecuteReader();

                while (data.Read()) {
                    HowToListItem howTo = new HowToListItem()
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
