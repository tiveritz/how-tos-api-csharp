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
        private string GetSubstepsQuery = @"
            SELECT StepsUriIds.uri_id, Steps.title,
            CASE
                WHEN Steps.id IN (SELECT DISTINCT super_id FROM Super) THEN true ELSE false
            END AS is_super
            FROM Steps
            JOIN StepsUriIds ON StepsUriIds.step_id = Steps.id
            JOIN Super ON Super.step_id = Steps.id
            WHERE Super.super_id = (
                SELECT step_id
                FROM StepsUriIds
                WHERE uri_id=@uriId)
            ORDER BY Super.pos";

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

        // -- private helper methods ------------------------------------------
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

            List<StepsOrderItem> steps = QuerySteps(cmd);

            foreach (StepsOrderItem step in steps)
            {
                CreateSubstepTree(step);
            }

            return steps;
        }

        private List<StepsOrderItem> QuerySteps(MySqlCommand cmd)
        {
            List<StepsOrderItem> steps = new List<StepsOrderItem>();

            try
            {
                Db.Connection.Open();
                cmd.Prepare();
                MySqlDataReader data = cmd.ExecuteReader();

                while (data.Read()) {
                    StepsOrderItem step = new StepsOrderItem()
                        {
                        Title = data.GetString(1),
                        IsSuper = data.GetBoolean(2)
                        };
                    step.SetId(data.GetString(0));
                    steps.Add(step);
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
            return steps;
        }

        // Recursively create the substep tree
        private StepsOrderItem CreateSubstepTree(StepsOrderItem superstep)
        {
            MySqlCommand cmd = Db.Connection.CreateCommand();
            cmd.CommandText = GetSubstepsQuery;
            cmd.Parameters.AddWithValue("@uriId", superstep.Id);
            
            superstep.Substeps = QuerySteps(cmd);

            if (superstep.Substeps == null)
            {
                return superstep;
            }

            foreach (StepsOrderItem substep in superstep.Substeps)
            {
                StepsOrderItem htoi = CreateSubstepTree(substep);
            }
            return superstep;
        }
    }
}
