using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Mvc;


namespace HowTosApi.Controllers
{
    public class StepsQuery
    {
        private AppDb Db;
        private string GetAllQuery = @"
            SELECT StepsUriIds.uri_id, Steps.title, Steps.ts_create, Steps.ts_update,
            CASE
                WHEN Steps.id IN (SELECT DISTINCT super_id FROM Super) THEN true ELSE false
            END AS is_super
            FROM Steps
            JOIN StepsUriIds ON Steps.id=StepsUriIds.step_id;";
        private string CreateQuery = @"
            INSERT INTO Steps (title)
            VALUES (@title)";
        private string MapUriId = @"
            INSERT INTO StepsUriIds (step_id, uri_id)
            VALUES (@id, @uriId)";
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

        public StepsQuery(AppDb db)
        {
            Db = db;
        }

        public List<StepListItem> GetAll()
        {
            MySqlCommand cmd = Db.Connection.CreateCommand();
            cmd.CommandText = GetAllQuery;

            List<StepListItem> steps = QueryRead(cmd);

            return steps;
        }

        public string CreateStep(CreateStep createStep)
        {
            // Writes new HowTo to db
            MySqlCommand createStepCmd = Db.Connection.CreateCommand();
            createStepCmd.CommandText = CreateQuery;
            createStepCmd.Parameters.AddWithValue("@title", createStep.Title);

            int id = Db.ExecuteNoneQueryAndGetId(createStepCmd);

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

        private List<StepListItem> QueryRead(MySqlCommand cmd)
        {
            List<StepListItem> steps = new List<StepListItem>();

            try
            {
                Db.Connection.Open();
                cmd.Prepare();
                MySqlDataReader data = cmd.ExecuteReader();

                while (data.Read()) {
                    StepListItem step = new StepListItem()
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

        internal List<StepListItem> GetStepsForHowTo(string uriId)
        {
            MySqlCommand cmd = Db.Connection.CreateCommand();
            cmd.CommandText = GetStepsForHowToQuery;
            cmd.Parameters.AddWithValue("@uriId", uriId);

            List<StepListItem> steps = QueryRead(cmd);

            return steps;
        }
    }
}