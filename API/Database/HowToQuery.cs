using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;


namespace HowTosApi.Controllers
{
    public class HowToQuery
    {
        private AppDb Db;
        private string GetOneQuery = @"
            SELECT HowTosUriIds.uri_id, HowTos.title, HowTos.ts_create, HowTos.ts_update
            FROM HowTos
            JOIN HowTosUriIds ON HowTos.id=HowTosUriIds.how_to_id
            WHERE uri_id=@uriId";
        private string GetStepsQuery = @"
            SELECT HowTosSteps.pos, StepsUriIds.uri_id, Steps.title,
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

        public HowTo GetOne(string uriId)
        {
            MySqlCommand cmd = Db.Connection.CreateCommand();
            cmd.CommandText = GetOneQuery;
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
                    string uriId = data.GetString(0);
                    HowTo howTo = new HowTo()
                        {
                        Title = data.GetString(1)
                        };
                    howTo.SetId(uriId);
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

        private List<HowToSteps> GetSteps(string uriId)
        {
            MySqlCommand cmd = Db.Connection.CreateCommand();
            cmd.CommandText = GetStepsQuery;
            cmd.Parameters.AddWithValue("@uriId", uriId);

            List<HowToSteps> steps = QueryStep(cmd);

            return steps;
        }

        private List<HowToSteps> QueryStep(MySqlCommand cmd)
        {
            List<HowToSteps> steps = new List<HowToSteps>();

            try
            {
                Db.Connection.Open();
                cmd.Prepare();
                MySqlDataReader data = cmd.ExecuteReader();

                while (data.Read()) {
                    string uriId = data.GetString(0);
                    HowToSteps step = new HowToSteps()
                        {
                        Pos = data.GetInt32(0),
                        Title = data.GetString(2),
                        IsSuper = data.GetBoolean(3)
                        };
                    step.SetId(data.GetString(1));
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
