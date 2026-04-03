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
        public string HostStarName { get; set; }
        public int PlayerID { get; set; }
        public string Username { get; set; }  // Joined from Users table

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
            // PlayerID in Simulations is the FK linking to Users.UserID
            string sql = "INSERT INTO Simulations (PlanetID, FitScore, TimeTakenSec, PlayerID) VALUES (?, ?, ?, ?)";
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            using (OleDbCommand cmd = new OleDbCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@PlanetID", planetID);
                cmd.Parameters.AddWithValue("@FitScore", score);
                cmd.Parameters.AddWithValue("@TimeTakenSec", timeTakenSec);
                cmd.Parameters.AddWithValue("@PlayerID", UserSession.CurrentUserID);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Returns all results for the user, joined with Users table to show Username.
        /// Logged-in: all attempts for that UserID.
        /// Guest: only this session's attempts (PlayerID = -1).
        /// </summary>
        public List<FitLineResult> GetAllResults()
        {
            List<FitLineResult> results = new List<FitLineResult>();

            string whereClause = UserSession.IsLoggedIn
                ? "WHERE s.PlayerID = " + UserSession.CurrentUserID
                : "WHERE (s.PlayerID = -1 OR s.PlayerID IS NULL)";

            // JOIN Simulations -> tblPlanet -> tblStar -> Users
            // Users is LEFT JOIN because guest rows have no matching User
            string sql =
                "SELECT s.SimulationID, s.FitScore, s.TimeTakenSec, " +
                "       p.pl_Name, st.HostName, " +
                "       s.PlayerID, u.Username " +
                "FROM (((Simulations s " +
                "  LEFT JOIN tblPlanet p ON s.PlanetID = p.PlanetID) " +
                "  LEFT JOIN tblStar st ON p.StarID = st.StarID) " +
                "  LEFT JOIN Users u ON s.PlayerID = u.UserID) " +
                whereClause + " ORDER BY s.SimulationID DESC";

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
                                SimulationID  = reader.GetInt32(0),
                                FitScore      = reader.IsDBNull(1) ? 0.0   : reader.GetDouble(1),
                                TimeTakenSec  = reader.IsDBNull(2) ? 0     : reader.GetInt32(2),
                                PlanetName    = reader.IsDBNull(3) ? "Unknown" : reader.GetString(3),
                                HostStarName  = reader.IsDBNull(4) ? "Unknown" : reader.GetString(4),
                                PlayerID      = reader.IsDBNull(5) ? -1    : reader.GetInt32(5),
                                Username      = reader.IsDBNull(6) ? "Guest" : reader.GetString(6)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not fetch results: " + ex.Message);
            }

            return results;
        }

        public (double AvgScore, double AvgTime) GetAverages()
        {
            string whereClause = UserSession.IsLoggedIn
                ? "WHERE PlayerID = " + UserSession.CurrentUserID
                : "WHERE (PlayerID = -1 OR PlayerID IS NULL)";

            string sql = "SELECT AVG(FitScore), AVG(TimeTakenSec) FROM Simulations " + whereClause;
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
                            double avgTime  = reader.IsDBNull(1) ? 0.0 : reader.GetDouble(1);
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
