using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;


namespace HowTosApi.Controllers
{
    public class HowToQuery
    {
        private AppDb Db;
        private string GetHowToByIdQuery = @"
            SELECT HowTosUriIds.uri_id, HowTos.title, HowTos.ts_create, HowTos.ts_update
            FROM HowTos
            JOIN HowTosUriIds ON HowTos.id=HowTosUriIds.how_to_id
            WHERE uri_id=@uriId";
        private string GetStepsQuery = @"
            SELECT StepsUriIds.uri_id, Steps.title,
            CASE
                WHEN Steps.id IN (SELECT DISTINCT super_id FROM Super) THEN true ELSE false
            END AS is_super
            FROM Steps
            JOIN StepsUriIds ON StepsUriIds.step_id = Steps.id
            JOIN HowTosSteps ON HowTosSteps.step_id = Steps.id
            WHERE HowTosSteps.how_to_id = (
                SELECT how_to_id
                FROM HowTosUriIds
                WHERE uri_id=@uriId)
            ORDER BY HowTosSteps.pos;";

        public HowToQuery(AppDb db)
        {
            Db = db;
        }

        public HowTo GetHowToById(string uriId)
        {
            MySqlCommand cmd = Db.Connection.CreateCommand();
            cmd.CommandText = GetHowToByIdQuery;
            cmd.Parameters.AddWithValue("@uriId", uriId);

            List<HowTo> howTos = QueryHowTo(cmd);

            if (howTos.Count > 0)
            {
                HowTo howTo = howTos[0];
                howTo.Steps = GetSteps(uriId);
                return howTo;
            }
            return null;

        }

        private List<HowTo> QueryHowTo(MySqlCommand cmd)
        {
            List<HowTo> howTos = new List<HowTo>();

            try
            {
                Db.Connection.Open();
                cmd.Prepare();
                MySqlDataReader data = cmd.ExecuteReader();

                while (data.Read()) {
                    HowTo howTo = new HowTo() { Title = data.GetString(1) };
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

        private List<StepsOrderItem> GetSteps(string uriId)
        {
            MySqlCommand cmd = Db.Connection.CreateCommand();
            cmd.CommandText = GetStepsQuery;
            cmd.Parameters.AddWithValue("@uriId", uriId);

            Substeps substeps = new Substeps(Db);
            List<StepsOrderItem> steps = substeps.QuerySubsteps(cmd);

            foreach (StepsOrderItem step in steps)
            {
                substeps.CreateSubstepTree(step);
            }

            return steps;
        }
    }
}
