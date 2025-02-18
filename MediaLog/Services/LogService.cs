using Microsoft.Data.Sqlite;
using System;

namespace MediaLog.Services
{
    public class LogService
    {
        private const string DatabasePath = "MediaLog.db";

        public LogService()
        {
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using var connection = new SqliteConnection($"Data Source={DatabasePath}");
            connection.Open();

            string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS MediaLog (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT NOT NULL,
                    Type TEXT NOT NULL
                )";

            using var command = new SqliteCommand(createTableQuery, connection);
            command.ExecuteNonQuery();
        }

        public void AddToLog(string title, string type)
        {
            using var connection = new SqliteConnection($"Data Source={DatabasePath}");
            connection.Open();

            string insertQuery = "INSERT INTO MediaLog (Title, Type) VALUES (@Title, @Type)";
            using var command = new SqliteCommand(insertQuery, connection);
            command.Parameters.AddWithValue("@Title", title);
            command.Parameters.AddWithValue("@Type", type);
            command.ExecuteNonQuery();
        }
    }
}
