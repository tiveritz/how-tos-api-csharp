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
                    CASE
                        WHEN (
                            SELECT COUNT(step_id)
                                FROM HowTosSteps hts
                                WHERE how_to_id = (
                                    SELECT how_to_id
                                    FROM HowTosUriIds
                                    WHERE uri_id=@howToUriId
                                )
                            ) = 0
                        THEN 0
                        ELSE (
                        SELECT MAX(pos) + 1
                            FROM HowTosSteps hts
                            WHERE how_to_id = (
                                SELECT how_to_id
                                FROM HowTosUriIds
                                WHERE uri_id=@howToUriId
                                )
                            )
                    END
                );";
        public string LinkSubstepToSuperQuery = @"
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
                    CASE
                        WHEN (
                            SELECT COUNT(step_id)
                                FROM Super s
                                    WHERE super_id = (
                                        SELECT step_id
                                        FROM StepsUriIds
                                        WHERE uri_id=@superUriId
                                    )
                            ) = 0
                        THEN 0
                        ELSE (
                        SELECT MAX(pos) + 1
                            FROM Super s
                                WHERE super_id = (
                                    SELECT step_id
                                    FROM StepsUriIds
                                    WHERE uri_id=@superUriId
                                )
                            )
                    END
                );";
        public string GetHowToSubstepUriIdQuery = @"
            SELECT uri_id
            FROM HowTosSteps
            JOIN StepsUriIds ON StepsUriIds.step_id=HowTosSteps.step_id
            WHERE how_to_id = (
                SELECT how_to_id
                FROM HowTosUriIds
                WHERE uri_id=@howToUriId
            ) AND uri_id=@stepUriId;";
        public string GetSuperSubstepUriIdQuery = @"
            SELECT uri_id
            FROM Super
            JOIN StepsUriIds ON StepsUriIds.step_id=Super.step_id
            WHERE super_id = (
                SELECT step_id
                FROM StepsUriIds
                WHERE uri_id=@superUriId
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
        public string DeleteStepFromSuperQuery = @"
            DELETE FROM Super
            WHERE super_id = (
                SELECT step_id
                FROM StepsUriIds
                WHERE uri_id=@superUriId
            ) AND step_id = (
                SELECT step_id
                FROM StepsUriIds
                WHERE uri_id=@stepUriId
            );";
        public string ReorderHowToStepsDownQuery = @"
            SELECT @step:=step_id
            FROM HowTosSteps
            WHERE how_to_id = (
                SELECT how_to_id
                FROM HowTosUriIds
                WHERE uri_id=@howToUriId)
                AND pos = @oldIndex;
            # then
            UPDATE HowTosSteps
            SET pos = pos - 1
            WHERE how_to_id = (
                SELECT how_to_id
                FROM HowTosUriIds
                WHERE uri_id=@howToUriId)
            AND pos > @oldIndex AND pos <= @newIndex;
            # then
            UPDATE HowTosSteps
            SET pos = @newIndex # new position
            WHERE how_to_id = (
                SELECT how_to_id
                FROM HowTosUriIds
                WHERE uri_id=@howToUriId)
            AND step_id = @step;";
        public string ReorderHowToStepsUpQuery = @"
            SELECT @step:=step_id
            FROM HowTosSteps
            WHERE how_to_id = (
                SELECT how_to_id
                FROM HowTosUriIds
                WHERE uri_id=@howToUriId)
                AND pos = @oldIndex;
            # then    
            UPDATE HowTosSteps
            SET pos = pos + 1
            WHERE how_to_id = (
                SELECT how_to_id
                FROM HowTosUriIds
                WHERE uri_id=@howToUriId)
            AND pos < @oldIndex AND pos >= @newIndex;
            # then
            UPDATE HowTosSteps
            SET pos = @newIndex	# new position
            WHERE how_to_id = (
                SELECT how_to_id
                FROM HowTosUriIds
                WHERE uri_id=@howToUriId)
            AND step_id = @step;";
        public string ReorderSuperStepsDownQuery = @"
            SELECT @step:=step_id
            FROM Super
            WHERE super_id = (
                SELECT step_id
                FROM StepsUriIds
                WHERE uri_id=@superUriId)
                AND pos = @oldIndex;
            # then
            UPDATE Super
            SET pos = pos - 1
            WHERE super_id = (
                SELECT step_id
                FROM StepsUriIds
                WHERE uri_id=@superUriId)
            AND pos > @oldIndex AND pos <= @newIndex;
            # then
            UPDATE Super
            SET pos = @newIndex
            WHERE super_id = (
                SELECT step_id
                FROM StepsUriIds
                WHERE uri_id=@superUriId)
            AND step_id = @step;";
        public string ReorderSuperStepsUpQuery = @"
            SELECT @step:=step_id
            FROM Super
            WHERE super_id = (
                SELECT step_id
                FROM StepsUriIds
                WHERE uri_id=@superUriId)
                AND pos = @oldIndex;
            # then    
            UPDATE Super
            SET pos = pos + 1
            WHERE super_id = (
                SELECT step_id
                FROM StepsUriIds
                WHERE uri_id=@superUriId)
            AND pos < @oldIndex AND pos >= @newIndex;
            # then
            UPDATE Super
            SET pos = @newIndex
            WHERE super_id = (
                SELECT step_id
                FROM StepsUriIds
                WHERE uri_id=@superUriId)
            AND step_id = @step;";

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
            cmd.CommandText = LinkSubstepToSuperQuery;
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

        public void DeleteStepFromSuper(string superUriId, string stepUriId)
        {
            MySqlCommand cmd = Db.Connection.CreateCommand();
            cmd.CommandText = DeleteStepFromSuperQuery;
            cmd.Parameters.AddWithValue("@superUriId", superUriId);
            cmd.Parameters.AddWithValue("@stepUriId", stepUriId);

            Db.ExecuteNonQuery(cmd);
        }

        public bool StepLinkedToHowTo(string howToUriId, string stepUriId)
        {
            MySqlCommand cmd = Db.Connection.CreateCommand();
            cmd.CommandText = GetHowToSubstepUriIdQuery;
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

        public bool StepLinkedToSuper(string superUriId, string stepUriId)
        {
            MySqlCommand cmd = Db.Connection.CreateCommand();
            cmd.CommandText = GetSuperSubstepUriIdQuery;
            cmd.Parameters.AddWithValue("@superUriId", superUriId);
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

        public void changeHowToOrder(string howToUriId, ChangeOrder changeOrder)
        {
            int oldIndex = changeOrder.OldIndex;
            int newIndex = changeOrder.NewIndex;

            MySqlCommand cmd = Db.Connection.CreateCommand();
            cmd.CommandText = oldIndex < newIndex ? ReorderHowToStepsDownQuery : ReorderHowToStepsUpQuery;
            cmd.Parameters.AddWithValue("@howToUriId", howToUriId);
            cmd.Parameters.AddWithValue("@oldIndex", oldIndex);
            cmd.Parameters.AddWithValue("@newIndex", newIndex);

            Db.ExecuteNonQuery(cmd);
        }

        public void changeSuperOrder(string stepUriId, ChangeOrder changeOrder)
        {
            int oldIndex = changeOrder.OldIndex;
            int newIndex = changeOrder.NewIndex;

            MySqlCommand cmd = Db.Connection.CreateCommand();
            cmd.CommandText = oldIndex < newIndex ? ReorderSuperStepsDownQuery : ReorderSuperStepsUpQuery;
            cmd.Parameters.AddWithValue("@superUriId", stepUriId);
            cmd.Parameters.AddWithValue("@oldIndex", oldIndex);
            cmd.Parameters.AddWithValue("@newIndex", newIndex);

            Db.ExecuteNonQuery(cmd);
        }
    }
}
