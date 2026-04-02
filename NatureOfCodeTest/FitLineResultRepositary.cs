using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace NatureOfCodeTest
{
    public class FitLineResult
    {
        public int SimulationID { get; set; }
        public double FitScore { get; set; }
        public int TimeTakenSec { get; set; }
        public int Timestamp { get; set; }
        
        public DateTime DatePlayed 
        {
            get 
            {
                DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                return dtDateTime.AddSeconds(Timestamp).ToLocalTime();
            }
        }
    }

    public class FitLineResultRepositary
    {
        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source = " + Environment.CurrentDirectory + @"\StellerWobble.accdb";

        public void AddResult(double score, int timeTakenSec)
        {
            int unixTimestamp = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            string sql = "INSERT INTO Simulations (Timestamp, FitScore, TimeTakenSec, IsFitLineGame) VALUES (?, ?, ?, ?)";
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            using (OleDbCommand cmd = new OleDbCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Timestamp", unixTimestamp);
                cmd.Parameters.AddWithValue("@FitScore", score);
                cmd.Parameters.AddWithValue("@TimeTakenSec", timeTakenSec);
                cmd.Parameters.AddWithValue("@IsFitLineGame", true);
                
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<FitLineResult> GetAllResults()
        {
            List<FitLineResult> results = new List<FitLineResult>();
            string sql = "SELECT SimulationID, Timestamp, FitScore, TimeTakenSec FROM Simulations WHERE IsFitLineGame = True ORDER BY SimulationID DESC";
            
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                using (OleDbCommand cmd = new OleDbCommand(sql, conn))
                {
                    conn.Open();
                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(new FitLineResult
                            {
                                SimulationID = reader.GetInt32(0),
                                Timestamp = reader.IsDBNull(1) ? 0 : reader.GetInt32(1),
                                FitScore = reader.IsDBNull(2) ? 0.0 : reader.GetDouble(2),
                                TimeTakenSec = reader.IsDBNull(3) ? 0 : reader.GetInt32(3)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // If table is missing column or other issues, return empty for now
                Console.WriteLine("Could not fetch results: " + ex.Message);
            }
            
            return results;
        }
    }
}
