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
        public string PlanetName { get; set; }
        
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

        public void AddResult(int planetID, double score, int timeTakenSec)
        {
           

            string sql = "INSERT INTO Simulations (PlanetID, FitScore, TimeTakenSec, IsFitLineGame) VALUES (?, ?, ?, ?)";
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            using (OleDbCommand cmd = new OleDbCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@PlanetID", planetID);
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
            string sql = "SELECT s.SimulationID, s.FitScore, s.TimeTakenSec, p.pl_Name FROM Simulations s LEFT JOIN tblPlanet p ON s.PlanetID = p.PlanetID WHERE s.IsFitLineGame = True ORDER BY s.SimulationID DESC";
            
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
                                FitScore = reader.IsDBNull(1) ? 0.0 : reader.GetDouble(1),
                                TimeTakenSec = reader.IsDBNull(2) ? 0 : reader.GetInt32(2),
                                PlanetName = reader.IsDBNull(3) ? "Unknown" : reader.GetString(3)
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
        public (double AvgScore, double AvgTime) GetAverages()
        {
            string sql = "SELECT AVG(FitScore), AVG(TimeTakenSec) FROM Simulations WHERE IsFitLineGame = True";
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                using (OleDbCommand cmd = new OleDbCommand(sql, conn))
                {
                    conn.Open();
                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            double avgScore = reader.IsDBNull(0) ? 0.0 : reader.GetDouble(0);
                            double avgTime = reader.IsDBNull(1) ? 0.0 : reader.GetDouble(1);
                            return (avgScore, avgTime);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not fetch averages: " + ex.Message);
            }
            return (0.0, 0.0);
        }
    }
}
