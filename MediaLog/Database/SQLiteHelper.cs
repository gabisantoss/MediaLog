using Microsoft.Data.Sqlite;
using System.IO;

namespace MediaLog.Database
{
    public static class SQLiteHelper
    {
        private static readonly string dbPath = "MediaLog.db";

        public static void InitializeDatabase()
        {
            if (!File.Exists(dbPath))
            {
                using (var conn = new SqliteConnection($"Data Source={dbPath}"))
                {
                    conn.Open();
                    string createTableQuery = @"CREATE TABLE IF NOT EXISTS MediaLog (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Title TEXT NOT NULL,
                        Type TEXT NOT NULL, 
                        Rating INTEGER, 
                        Review TEXT,
                        DateConsumed DATETIME DEFAULT CURRENT_TIMESTAMP
                    );";

                    using (var command = new SqliteCommand(createTableQuery, conn))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public static void InsertMedia(string title, string type, int rating, string review)
        {
            using (var conn = new SqliteConnection($"Data Source={dbPath}"))
            {
                conn.Open();
                string insertQuery = "INSERT INTO MediaLog (Title, Type, Rating, Review) VALUES (@Title, @Type, @Rating, @Review)";
                using (var command = new SqliteCommand(insertQuery, conn))
                {
                    command.Parameters.AddWithValue("@Title", title);
                    command.Parameters.AddWithValue("@Type", type);
                    command.Parameters.AddWithValue("@Rating", rating);
                    command.Parameters.AddWithValue("@Review", review);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static List<string> GetAllMedia()
        {
            var mediaList = new List<string>();

            using (var conn = new SqliteConnection($"Data Source={dbPath}; Version=3;"))
            {
                conn.Open();
                string selectQuery = "SELECT Title, Type, Rating, Review FROM MediaLog";
                using (var command = new SqliteCommand(selectQuery, conn))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            mediaList.Add($"{reader["Title"]} ({reader["Type"]}) - Rating: {reader["Rating"]}/10 - {reader["Review"]}");
                        }
                    }
                }
            }
            return mediaList;
        }
    }
}
