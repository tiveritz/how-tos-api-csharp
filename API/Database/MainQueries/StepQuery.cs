using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;


namespace HowTosApi.Controllers
{
    public class StepQuery
    {
        private AppDb Db;
        private string GetStepByIdQuery = @"
            SELECT * FROM GetSteps WHERE uri_id=@uriId;";
        private string DeleteStepQuery = @"
            DELETE FROM Steps
            WHERE id = (
                SELECT step_id
                FROM StepsUriIds
                WHERE uri_id=@uriId
            );";
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
                SubstepsQuery substep = new SubstepsQuery(Db);
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

        public bool StepExists(string uriId) {
            if (GetStepById(uriId) == null) {
                return false;
            }
            return true;
        }
    }
}
