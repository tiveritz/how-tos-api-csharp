using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;


namespace HowTosApi.Controllers
{
    public class StatisticsQuery
    {
        private AppDb Db;
        private string HowTosCountQuery = @"
            SELECT COUNT(id) as how_tos_count
            FROM HowTos;";
        private string StepsCountQuery = @"
            SELECT COUNT(id) as steps_count
            FROM Steps;";
        private string SuperStepsCountQuery = @"
            SELECT COUNT(DISTINCT(super_id))
            FROM Super;";
        private string SubStepsCountQuery = @"
            SELECT COUNT(id) as substeps_count
            FROM Steps
            WHERE id NOT IN (
                SELECT DISTINCT super_id
                FROM Super
            );";

        public StatisticsQuery(AppDb db)
        {
            Db = db;
        }

        public Statistic GetStatistics()
        {
            MySqlCommand howTosCountCmd = Db.Connection.CreateCommand();
            MySqlCommand stepsCountCmd = Db.Connection.CreateCommand();
            MySqlCommand superStepsCountCmd = Db.Connection.CreateCommand();
            MySqlCommand subStepsCountCmd = Db.Connection.CreateCommand();
            howTosCountCmd.CommandText = HowTosCountQuery;
            stepsCountCmd.CommandText = StepsCountQuery;
            superStepsCountCmd.CommandText = SuperStepsCountQuery;
            subStepsCountCmd.CommandText = SubStepsCountQuery;

            Statistic stat = new Statistic
            {
            HowTosCount = GetStatistic(howTosCountCmd),
            StepsCount = GetStatistic(stepsCountCmd),
            SuperStepsCount = GetStatistic(superStepsCountCmd),
            SubStepsCount = GetStatistic(subStepsCountCmd)
            };

            return stat;
        }

        private int GetStatistic(MySqlCommand cmd)
        {
            List<int> stat = new List<int>();

            try
            {
                Db.Connection.Open();
                cmd.Prepare();
                MySqlDataReader data = cmd.ExecuteReader();

                while (data.Read()) {
                    stat.Add(data.GetInt32(0));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
            Db.Connection.Close();
            }
            return  stat.Count > 0 ? stat[0] : -1; // Find better error handling
        }
    }
}
