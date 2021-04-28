using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;


namespace HowTosApi.Controllers
{
    public class SubstepQuery
    {
        private AppDb Db;
        public string LinkStepToHowToQuery = @"
            INSERT INTO HowTosSteps
            SET
                how_to_id = (
                    SELECT how_to_id
                    FROM HowTosUriIds
                    WHERE uri_id=@howToUriId
                ),
                step_id = (
                    SELECT step_id
                    FROM StepsUriIds
                    WHERE uri_id=@stepUriId
                ),
                pos = (
                    SELECT MAX(pos) + 1
                    FROM HowTosSteps hts
                    WHERE how_to_id = (
                        SELECT how_to_id
                        FROM HowTosUriIds
                        WHERE uri_id=@howToUriId
                    )
                );";
        
        public string LinkSubstepToHowToQuery = @"
            INSERT INTO Super
            SET
                super_id = (
                    SELECT step_id
                    FROM StepsUriIds
                    WHERE uri_id=@superUriId
                ),
                step_id = (
                    SELECT step_id
                    FROM StepsUriIds
                    WHERE uri_id=@stepUriId
                ),
                pos = (
                    SELECT MAX(pos) + 1
                    FROM Super s
                    WHERE super_id = (
                        SELECT step_id
                        FROM StepsUriIds
                        WHERE uri_id=@superUriId
                    )
                );";

        public string GetHowToSubstepUriIds = @"
            SELECT uri_id
            FROM HowTosSteps
            JOIN StepsUriIds ON StepsUriIds.step_id=HowTosSteps.step_id
            WHERE how_to_id = (
                SELECT how_to_id
                FROM HowTosUriIds
                WHERE uri_id=@howToUriId
            ) AND uri_id=@stepUriId;";
        
        public string DeleteStepFromHowToQuery = @"
            DELETE FROM HowTosSteps
            WHERE how_to_id = (
                SELECT how_to_id
                FROM HowTosUriIds
                WHERE uri_id=@howToUriId
            ) AND step_id = (
                SELECT step_id
                FROM StepsUriIds
                WHERE uri_id=@stepUriId
            );";

        public SubstepQuery(AppDb db)
        {
            Db = db;
        }

        public void linkStepToHowTo(string howToUriId, string stepUriId)
        {
            MySqlCommand cmd = Db.Connection.CreateCommand();
            cmd.CommandText = LinkStepToHowToQuery;
            cmd.Parameters.AddWithValue("@howToUriId", howToUriId);
            cmd.Parameters.AddWithValue("@stepUriId", stepUriId);

            Db.ExecuteNonQuery(cmd);
        }

        public void linkStepToSuper(string superUriId, string stepUriId)
        {
            MySqlCommand cmd = Db.Connection.CreateCommand();
            cmd.CommandText = LinkSubstepToHowToQuery;
            cmd.Parameters.AddWithValue("@superUriId", superUriId);
            cmd.Parameters.AddWithValue("@stepUriId", stepUriId);

            Db.ExecuteNonQuery(cmd);
        }

        public void DeleteStepFromHowTo(string howToUriId, string stepUriId)
        {
            MySqlCommand cmd = Db.Connection.CreateCommand();
            cmd.CommandText = DeleteStepFromHowToQuery;
            cmd.Parameters.AddWithValue("@howToUriId", howToUriId);
            cmd.Parameters.AddWithValue("@stepUriId", stepUriId);

            Db.ExecuteNonQuery(cmd);
        }

        public bool StepLinkedToHowTo(string howToUriId, string stepUriId)
        {
            MySqlCommand cmd = Db.Connection.CreateCommand();
            cmd.CommandText = GetHowToSubstepUriIds;
            cmd.Parameters.AddWithValue("@howToUriId", howToUriId);
            cmd.Parameters.AddWithValue("@stepUriId", stepUriId);

            List<string> steps = new List<string>();

            try
            {
                Db.Connection.Open();
                cmd.Prepare();
                MySqlDataReader data = cmd.ExecuteReader();

                while (data.Read()) {
                    string uriId = data.GetString(0);
                    steps.Add(uriId);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
            finally
            {
                Db.Connection.Close();
            }

            if (steps.Count == 1)
            {
                return true;
            }
            return false;
        }
    }
}
