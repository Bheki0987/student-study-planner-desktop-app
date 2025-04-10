using System;
using System.Data.SQLite;
using System.Security.Cryptography;
using System.Text;

namespace StudentStudyPlanner
{
    public static class UserManager
    {
        private const string ConnectionString = "Data Source=studyplanner.db;Version=3;";

        static UserManager()
        {
            InitializeDatabase();
        }

        private static void InitializeDatabase()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = @"
                        CREATE TABLE IF NOT EXISTS Users (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Username TEXT NOT NULL UNIQUE,
                            PasswordHash TEXT NOT NULL,
                            CreatedAt DATETIME NOT NULL
                        )";
                    command.ExecuteNonQuery();
                }
            }
        }

        public static (bool Success, string Message) RegisterUser(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return (false, "Username and password cannot be empty.");
            }

            string passwordHash = HashPassword(password);

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = "INSERT INTO Users (Username, PasswordHash, CreatedAt) VALUES (@username, @passwordHash, @createdAt)";
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@passwordHash", passwordHash);
                    command.Parameters.AddWithValue("@createdAt", DateTime.Now);

                    try
                    {
                        command.ExecuteNonQuery();
                        return (true, "User registered successfully.");
                    }
                    catch (SQLiteException ex)
                    {
                        if (ex.ResultCode == SQLiteErrorCode.Constraint && ex.Message.Contains("UNIQUE"))
                        {
                            return (false, "Username already exists. Please choose a different username.");
                        }
                        else
                        {
                            // Log the exception for debugging purposes
                            Console.WriteLine($"SQLite error: {ex.Message}");
                            return (false, "An error occurred during registration. Please try again.");
                        }
                    }
                }
            }
        }

        public static bool AuthenticateUser(string username, string password)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = "SELECT PasswordHash FROM Users WHERE Username = @username";
                    command.Parameters.AddWithValue("@username", username);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string storedHash = reader.GetString(0);
                            return VerifyPassword(password, storedHash);
                        }
                    }
                }
            }
            return false;
        }

        private static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private static bool VerifyPassword(string password, string storedHash)
        {
            string passwordHash = HashPassword(password);
            return passwordHash == storedHash;
        }
    }
}