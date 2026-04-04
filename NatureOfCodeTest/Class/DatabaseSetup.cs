using System;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

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

                // If there is any null user rows treat as guest (-1)
                TryExecute(conn, "UPDATE Simulations SET UserID = -1 WHERE UserID IS NULL");

                // Delete all guest / unowned rows from previous sessions
                TryExecute(conn, "DELETE FROM Simulations WHERE UserID <= 0");
            }
        }

        private static void TryExecute(OleDbConnection conn, string sql)
        {
            // try to execute the SQL.
            try
            {
                using (OleDbCommand cmd = new OleDbCommand(sql, conn))
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
