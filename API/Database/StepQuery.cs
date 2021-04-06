using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;


namespace HowTosApi.Controllers
{
    public class StepQuery
    {
        private AppDb Db;
        private string GetOneQuery = @"
            SELECT StepsUriIds.uri_id, Steps.title, Steps.ts_create, Steps.ts_update,
            CASE
                WHEN Steps.id IN (SELECT DISTINCT super_id FROM Super) THEN true ELSE false
            END AS is_super
            FROM Steps
            JOIN StepsUriIds ON Steps.id=StepsUriIds.step_id
            WHERE uri_id=@uriId;";
        public string GetSubstepsQuery = @"
            SELECT Super.pos, StepsUriIds.uri_id, Steps.title,
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

        public Step GetOne(string uriId)
        {
            MySqlCommand cmd = Db.Connection.CreateCommand();
            cmd.CommandText = GetOneQuery;
            cmd.Parameters.AddWithValue("@uriId", uriId);

            List<Step> steps = QuerySteps(cmd);

            if (steps.Count > 0)
            {
                Step step = steps[0];
                step.Steps = GetSubsteps(uriId);
                return step;
            }
            return null;
        }

        private List<SubstepListItem> GetSubsteps(string uriId)
        {
            MySqlCommand cmd = Db.Connection.CreateCommand();
            cmd.CommandText = GetSubstepsQuery;
            cmd.Parameters.AddWithValue("@uriId", uriId);

            List<SubstepListItem> substeps = QuerySubsteps(cmd);

            return substeps;
        }

        private List<Step> QuerySteps(MySqlCommand cmd)
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
        private List<SubstepListItem> QuerySubsteps(MySqlCommand cmd)
        {
            List<SubstepListItem> steps = new List<SubstepListItem>();

            try
            {
                Db.Connection.Open();
                cmd.Prepare();
                MySqlDataReader data = cmd.ExecuteReader();

                while (data.Read()) {
                    string uriId = data.GetString(1);
                    SubstepListItem substep = new SubstepListItem()
                        {
                        Pos = data.GetInt32(0),
                        Title = data.GetString(2),
                        IsSuper = data.GetBoolean(3)
                        };
                    substep.SetId(uriId);
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
    }
}
