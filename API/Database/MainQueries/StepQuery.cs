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
        private string DeleteStepQuery = @"
            DELETE FROM StepsUriIds
            WHERE uri_id=@uriId;";
        private string GetSuperQuery = @"
            SELECT Steps.title
            FROM Super
            JOIN Steps ON Super.super_id = Steps.id
            WHERE step_id = 4;";
        private string ChangeStepQuery = @"
            UPDATE Steps
            SET title = @title
            WHERE id = (
                SELECT step_id
                FROM StepsUriIds
                WHERE uri_id=@uriId
            );";

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
                Substeps substep = new Substeps(Db);
                step.Steps = substep.GetSubsteps(uriId);
                return step;
            }
            return null;
        }

        public void DeleteStep(string uriId)
        {
            MySqlCommand cmd = Db.Connection.CreateCommand();
            cmd.CommandText = DeleteStepQuery;
            cmd.Parameters.AddWithValue("@uriId", uriId);

            Db.ExecuteNonQuery(cmd);
        }

        public void ChangeStep(string uriId, ChangeStep changeStep)
        {
            MySqlCommand cmd = Db.Connection.CreateCommand();
            cmd.CommandText = ChangeStepQuery;
            cmd.Parameters.AddWithValue("@uriId", uriId);
            cmd.Parameters.AddWithValue("@title", changeStep.Title);

            Db.ExecuteNonQuery(cmd);
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
    }
}
