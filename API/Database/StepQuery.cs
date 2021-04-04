using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Mvc;


namespace HowTosApi.Controllers
{
    public class StepQuery
    {
        private AppDb Db;
        private string GetAllQuery = @"
            SELECT StepsUriIds.uri_id, Steps.title, Steps.ts_create, Steps.ts_update,
            CASE
                WHEN Steps.id IN (SELECT DISTINCT super_id FROM Super) THEN true ELSE false
            END AS is_super
            FROM Steps
            JOIN StepsUriIds ON Steps.id=StepsUriIds.step_id;";
        private string GetOneQuery = @"
            SELECT StepsUriIds.uri_id, Steps.title, Steps.ts_create, Steps.ts_update,
            CASE
                WHEN Steps.id IN (SELECT DISTINCT super_id FROM Super) THEN true ELSE false
            END AS is_super
            FROM Steps
            JOIN StepsUriIds ON Steps.id=StepsUriIds.step_id
            WHERE uri_id=@uriId;";
        private string GetStepsForHowToQuery = @"
                SELECT HowTosSteps.pos, StepsUriIds.uri_id, Steps.title, Steps.ts_create, Steps.ts_update,
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

        public StepQuery(AppDb db)
        {
            Db = db;
        }

        public List<Step> GetAll()
        {
            MySqlCommand cmd = Db.Connection.CreateCommand();
            cmd.CommandText = GetAllQuery;

            List<Step> steps = QueryRead(cmd);

            return steps;
        }

        public Step GetOne(string uriId)
        {
            MySqlCommand cmd = Db.Connection.CreateCommand();
            cmd.CommandText = GetOneQuery;
            cmd.Parameters.AddWithValue("@uriId", uriId);

            List<Step> steps = QueryRead(cmd);
            
            return steps.Count > 0 ? steps[0] : null;
        }

        private List<Step> QueryRead(MySqlCommand cmd)
        {
            List<Step> steps = new List<Step>();

            try
            {
                Db.Connection.Open();
                cmd.Prepare();
                MySqlDataReader data = cmd.ExecuteReader();

                while (data.Read()) {
                    Step step = new Step()
                        {
                        Title = data.GetString(1),
                        Created = data.GetDateTime(2),
                        Updated = data.GetDateTime(3),
                        IsSuper = data.GetBoolean(4)
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

        internal List<Step> GetStepsForHowTo(string uriId)
        {
            MySqlCommand cmd = Db.Connection.CreateCommand();
            cmd.CommandText = GetStepsForHowToQuery;
            cmd.Parameters.AddWithValue("@uriId", uriId);

            List<Step> steps = QueryRead(cmd);

            return steps;
        }
    }
}
