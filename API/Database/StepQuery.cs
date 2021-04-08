using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;


namespace HowTosApi.Controllers
{
    public class StepQuery
    {
        private AppDb Db;
        private string GetStepByIdQuery = @"
            SELECT StepsUriIds.uri_id, Steps.title, Steps.ts_create, Steps.ts_update,
            CASE
                WHEN Steps.id IN (SELECT DISTINCT super_id FROM Super) THEN true ELSE false
            END AS is_super
            FROM Steps
            JOIN StepsUriIds ON Steps.id=StepsUriIds.step_id
            WHERE uri_id=@uriId;";
        public string GetSubstepsQuery = @"
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

        public StepQuery(AppDb db)
        {
            Db = db;
        }

        public Step GetStepById(string uriId)
        {
            MySqlCommand cmd = Db.Connection.CreateCommand();
            cmd.CommandText = GetStepByIdQuery;
            cmd.Parameters.AddWithValue("@uriId", uriId);

            List<Step> steps = QueryStep(cmd);

            if (steps.Count > 0)
            {
                Step step = steps[0];
                step.Steps = GetSubsteps(uriId);
                return step;
            }
            return null;
        }

        private List<Step> QueryStep(MySqlCommand cmd)
        {
            List<Step> steps = new List<Step>();

            try
            {
                Db.Connection.Open();
                cmd.Prepare();
                MySqlDataReader data = cmd.ExecuteReader();

                while (data.Read()) {
                    string uriId = data.GetString(0);
                    Step step = new Step()
                        {
                        Title = data.GetString(1),
                        IsSuper = data.GetBoolean(4)
                        };
                    step.SetId(uriId);
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

        private List<StepsOrderItem> GetSubsteps(string uriId)
        {
            MySqlCommand cmd = Db.Connection.CreateCommand();
            cmd.CommandText = GetSubstepsQuery;
            cmd.Parameters.AddWithValue("@uriId", uriId);

            List<StepsOrderItem> steps = QuerySubsteps(cmd);

            foreach (StepsOrderItem step in steps)
            {
                CreateSubstepTree(step);
            }

            return steps;
        }

        private List<StepsOrderItem> QuerySubsteps(MySqlCommand cmd)
        {
            List<StepsOrderItem> steps = new List<StepsOrderItem>();

            try
            {
                Db.Connection.Open();
                cmd.Prepare();
                MySqlDataReader data = cmd.ExecuteReader();

                while (data.Read()) {
                    StepsOrderItem substep = new StepsOrderItem()
                        {
                        Title = data.GetString(1),
                        IsSuper = data.GetBoolean(2)
                        };
                    substep.SetId(data.GetString(0));
                    steps.Add(substep);
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
            
            superstep.Substeps = QuerySubsteps(cmd);

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
