using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.Collections.Generic;
using System;
using System.IO;

public class DatabaseManager : MonoBehaviour
{
    // Connection string for the SQLite database
    private static string connectionString;

    // This method is called when the script instance is being loaded
    private void Start()
    {
        // Define the path to the database folder within the Unity project
        string dbFolderPath = Path.Combine(Application.dataPath, "Database");

        // Create the database folder if it doesn't exist
        if (!Directory.Exists(dbFolderPath))
        {
            Directory.CreateDirectory(dbFolderPath);
        }

        // Define the full path to the SQLite database file
        string dbPath = Path.Combine(dbFolderPath, "wins.db");
        connectionString = $"URI=file:{dbPath}";
        print(dbPath);  // Print the database path for debugging purposes

        // Create the table if it doesn't already exist
        CreateTable();
    }

    // Method to create the WinRecord table in the database if it doesn't exist
    private void CreateTable()
    {
        // Open a connection to the database
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                // SQL command to create the table with columns for Id, PlayerName, Time, and Date
                command.CommandText = "CREATE TABLE IF NOT EXISTS WinRecord (Id INTEGER PRIMARY KEY AUTOINCREMENT, PlayerName TEXT, Time TEXT, Date TEXT)";
                command.ExecuteNonQuery();  // Execute the command without expecting any results
            }
        }
    }

    // Method to log a win into the database
    public static void LogWin(string playerName, string time)
    {
        // Open a connection to the database
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                // SQL command to insert a new win record into the table
                command.CommandText = "INSERT INTO WinRecord (PlayerName, Time, Date) VALUES (@PlayerName, @Time, @Date)";
                // Add parameters to the command to prevent SQL injection
                command.Parameters.AddWithValue("@PlayerName", playerName);
                command.Parameters.AddWithValue("@Time", time);
                command.Parameters.AddWithValue("@Date", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));  // Store the current UTC time

                command.ExecuteNonQuery();  // Execute the insert command
            }
        }
        Debug.Log("Win logged successfully");  // Log a message indicating the win was successfully recorded
    }

    // Method to retrieve all win records from the database
    public static List<WinRecord> GetWins()
    {
        // List to hold the retrieved win records
        List<WinRecord> wins = new List<WinRecord>();

        // Open a connection to the database
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                // SQL command to select all records from the WinRecord table
                command.CommandText = "SELECT * FROM WinRecord";
                using (IDataReader reader = command.ExecuteReader())
                {
                    // Loop through the results and add each win record to the list
                    while (reader.Read())
                    {
                        WinRecord win = new WinRecord
                        {
                            Id = reader.GetInt32(0),  // Retrieve the Id of the win record
                            PlayerName = reader.GetString(1),  // Retrieve the player's name
                            Time = reader.GetString(2),  // Retrieve the time of the win
                            Date = DateTime.Parse(reader.GetString(3))  // Retrieve and parse the date of the win
                        };
                        wins.Add(win);  // Add the win record to the list
                    }
                }
            }
        }

        // Return the list of win records
        return wins;
    }
}

// Class to represent a win record
public class WinRecord
{
    public int Id { get; set; }  // Unique identifier for the win record
    public string PlayerName { get; set; }  // Name of the player who won
    public string Time { get; set; }  // Time taken by the player to win
    public DateTime Date { get; set; }  // Date and time when the win was recorded
}
