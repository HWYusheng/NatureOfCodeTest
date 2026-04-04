using System;
using System.Data.OleDb;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace NatureOfCodeTest
{
    // Handles all database operations against the Users table.
    public class UserRepositary
    {
        private string connectionString =
            @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source = " +
            Environment.CurrentDirectory +
            @"\StellerWobble.accdb";

        // Returns a SHA-256 hex string for the given plain-text password.
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

        // Attempts to authenticate a user against the Users table.
        // If success, sets UserSession and returns (true, userID, username).
        // If fails, returns (false, -1, errorMessage).
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

        // Registers a new user in the Users table, then fetches their generated UserID.
        // If success, sets UserSession and returns (true, userID, null).
        // If fails, returns (false, -1, errorMessage).
        public (bool success, int userID, string errorMessage) Register(string username, string password)
        {
            if (UserExists(username))
            {
                return (false, -1, "Username already exists.");
            }

            string hash = HashPassword(password);
            string insertSql = "INSERT INTO Users (Username, PasswordHash) VALUES (?, ?)";

            try
            {
                // Insert the new user
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                using (OleDbCommand cmd = new OleDbCommand(insertSql, conn))
                {
                    cmd.Parameters.AddWithValue("?", username);
                    cmd.Parameters.AddWithValue("?", hash);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                // Fetch the newly generated UserID
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
                // Give a generic error for duplicate username 
                string msg = (ex.Message.Contains("duplicate") || ex.Message.Contains("unique") || ex.Errors.Count > 0)
                    ? "Username already exists."
                    : "Register error: " + ex.Message;

                return (false, -1, msg);
            }
        }

        // Checks if a username already exists in the Users table.
        public bool UserExists(string username)
        {
            string sql = "SELECT COUNT(*) FROM Users WHERE Username = ?";
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                using (OleDbCommand cmd = new OleDbCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("?", username);
                    conn.Open();
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
