using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.Collections.Generic;
using System;
using System.IO;

public class DatabaseManager : MonoBehaviour
{
    private static string connectionString;

    private void Start()
    {
        string dbFolderPath = Path.Combine(Application.dataPath, "Database");
        if (!Directory.Exists(dbFolderPath))
        {
            Directory.CreateDirectory(dbFolderPath);
        }
        string dbPath = Path.Combine(dbFolderPath, "wins.db");
        connectionString = $"URI=file:{dbPath}";
        print(dbPath);

        CreateTable();
    }

    private void CreateTable()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "CREATE TABLE IF NOT EXISTS WinRecord (Id INTEGER PRIMARY KEY AUTOINCREMENT, PlayerName TEXT, Time TEXT, Date TEXT)";
                command.ExecuteNonQuery();
            }
        }
    }

    public static void LogWin(string playerName, string time)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO WinRecord (PlayerName, Time, Date) VALUES (@PlayerName, @Time, @Date)";
                command.Parameters.AddWithValue("@PlayerName", playerName);
                command.Parameters.AddWithValue("@Time", time);
                command.Parameters.AddWithValue("@Date", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
                command.ExecuteNonQuery();
            }
        }
        Debug.Log("Win logged successfully");
    }

    public static List<WinRecord> GetWins()
    {
        List<WinRecord> wins = new List<WinRecord>();

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM WinRecord";
                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        WinRecord win = new WinRecord
                        {
                            Id = reader.GetInt32(0),
                            PlayerName = reader.GetString(1),
                            Time = reader.GetString(2),
                            Date = DateTime.Parse(reader.GetString(3))
                        };
                        wins.Add(win);
                    }
                }
            }
        }

        return wins;
    }
}

public class WinRecord
{
    public int Id { get; set; }
    public string PlayerName { get; set; }
    public string Time { get; set; }
    public DateTime Date { get; set; }
}
