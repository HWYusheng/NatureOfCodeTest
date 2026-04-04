using System;
using System.Data.OleDb;
using System.Security.Cryptography;
using System.Text;

namespace NatureOfCodeTest
{
    /// <summary>
    /// Handles all database operations against the Users table.
    /// Follows the same repository pattern as FitLineResultRepositary and CelesBodyRepositary.
    /// </summary>
    public class UserRepositary
    {
        private string connectionString =
            @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source = " +
            Environment.CurrentDirectory +
            @"\StellerWobble.accdb";

        // ─── Private Helpers ────────────────────────────────────────────────

        /// <summary>
        /// Returns a SHA-256 hex string for the given plain-text password.
        /// </summary>
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder sb = new StringBuilder();
                foreach (byte b in bytes)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }

        // ─── Public Methods ──────────────────────────────────────────────────

        /// <summary>
        /// Attempts to authenticate a user against the Users table.
        /// On success, sets UserSession and returns (true, userID, username).
        /// On failure, returns (false, -1, errorMessage).
        /// </summary>
        public (bool success, int userID, string username, string errorMessage) Login(string username, string password)
        {
            string hash = HashPassword(password);
            string sql = "SELECT UserID, Username FROM Users WHERE Username = ? AND PasswordHash = ?";

            try
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                using (OleDbCommand cmd = new OleDbCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("?", username);
                    cmd.Parameters.AddWithValue("?", hash);
                    conn.Open();

                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string name = reader.GetString(1);
                            return (true, id, name, null);
                        }
                        else
                        {
                            return (false, -1, null, "Invalid username or password.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return (false, -1, null, "Login error: " + ex.Message);
            }
        }

        /// <summary>
        /// Registers a new user in the Users table, then fetches their generated UserID.
        /// On success, sets UserSession and returns (true, userID, null).
        /// On failure, returns (false, -1, errorMessage).
        /// </summary>
        public (bool success, int userID, string errorMessage) Register(string username, string password)
        {
            string hash = HashPassword(password);
            string insertSql = "INSERT INTO Users (Username, PasswordHash) VALUES (?, ?)";

            try
            {
                // Step 1: Insert the new user
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                using (OleDbCommand cmd = new OleDbCommand(insertSql, conn))
                {
                    cmd.Parameters.AddWithValue("?", username);
                    cmd.Parameters.AddWithValue("?", hash);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                // Step 2: Fetch the newly generated UserID
                string selectSql = "SELECT UserID FROM Users WHERE Username = ?";
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                using (OleDbCommand cmd = new OleDbCommand(selectSql, conn))
                {
                    cmd.Parameters.AddWithValue("?", username);
                    conn.Open();

                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return (true, reader.GetInt32(0), null);
                        }
                    }
                }

                return (false, -1, "Registration succeeded but could not retrieve new UserID.");
            }
            catch (OleDbException ex)
            {
                // Access raises a generic error for unique-constraint violations
                string msg = (ex.Message.Contains("duplicate") || ex.Message.Contains("unique") || ex.Errors.Count > 0)
                    ? "Username already exists."
                    : "Register error: " + ex.Message;

                return (false, -1, msg);
            }
        }
    }
}
