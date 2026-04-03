using System;
using System.Data.OleDb;

namespace NatureOfCodeTest
{
    public static class DatabaseSetup
    {
        private static string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source = " + Environment.CurrentDirectory + @"\StellerWobble.accdb";

        public static void InitializeDb()
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();

                // 1. Create Users Table
                try
                {
                    string createUsers = "CREATE TABLE Users (UserID AUTOINCREMENT PRIMARY KEY, Username VARCHAR(255) UNIQUE, PasswordHash VARCHAR(255))";
                    using (OleDbCommand cmd = new OleDbCommand(createUsers, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (OleDbException)
                {
                    // Table already exists, ignore
                }

                // 2. Add PlayerID to Simulations
                try
                {
                    string addCol = "ALTER TABLE Simulations ADD COLUMN PlayerID INT";
                    using (OleDbCommand cmd = new OleDbCommand(addCol, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (OleDbException)
                {
                    // Column already exists, ignore
                }
                
                // Set default for existing records in Simulations to prevent null comparisons from failing
                try
                {
                    string updateOld = "UPDATE Simulations SET PlayerID = -1 WHERE PlayerID IS NULL";
                    using (OleDbCommand cmd = new OleDbCommand(updateOld, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                catch { }

                // 3. Clear temporary guest runs 
                try
                {
                    string clearGuests = "DELETE FROM Simulations WHERE PlayerID <= 0";
                    using (OleDbCommand cmd = new OleDbCommand(clearGuests, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (OleDbException)
                {
                }
            }
        }
    }
}
