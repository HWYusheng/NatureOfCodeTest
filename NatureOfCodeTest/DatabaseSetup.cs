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

                // 1. Create Users table if not exists
                TryExecute(conn, "CREATE TABLE Users (UserID AUTOINCREMENT PRIMARY KEY, Username VARCHAR(255), PasswordHash VARCHAR(255))");

                // 2. Add UserID column to Simulations (the FK column)
                TryExecute(conn, "ALTER TABLE Simulations ADD COLUMN UserID INTEGER");

                // 3. If old PlayerID column exists, migrate data across then we leave it (Access can't drop columns via SQL)
                TryExecute(conn, "UPDATE Simulations SET UserID = PlayerID WHERE UserID IS NULL AND PlayerID IS NOT NULL");

                // 4. Tag any remaining null rows as guest (-1)
                TryExecute(conn, "UPDATE Simulations SET UserID = -1 WHERE UserID IS NULL");

                // 5. Delete all guest / unowned rows from previous sessions
                TryExecute(conn, "DELETE FROM Simulations WHERE UserID <= 0");
            }
        }

        private static void TryExecute(OleDbConnection conn, string sql)
        {
            try
            {
                using (OleDbCommand cmd = new OleDbCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            catch (OleDbException)
            {
                // Silently ignore: column/table already exists, or optional migration step not applicable
            }
        }
    }
}
