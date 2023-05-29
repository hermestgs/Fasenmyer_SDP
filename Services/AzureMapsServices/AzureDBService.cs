using Npgsql;
using System.Data;
using System.Text.Json.Nodes;
using System.Text.Json;
using System;

// Used for retrieving data from the webapp database
namespace FasenmyerConference.Services.AzureMapsServices
{
    public class AzureDBService
    {
        // The connect string and requirements loaded from the config file
        private string _AzureDBConnStr = string.Empty;

        public AzureDBService()
        {

        }
        // Constructor that loads the connection string for the database
        public AzureDBService(string inputConnectionString)
        {
            _AzureDBConnStr = inputConnectionString;

        }

        // Loads the connection string for the database
        public void loadConnectionString(string inputConnectionString)
        {
            _AzureDBConnStr = inputConnectionString;
        }

        // Gets all the presentations for today from the database
        public void getPresentations(PresentationObserverBuilder Builder)
        {
            // Creates a database connection object
            using (var conn = new NpgsqlConnection(_AzureDBConnStr))
            {
                // Connects to the database
                //Console.WriteLine("Opening database connection...");
                conn.Open();

                // Uses SQL command to retrieve presentation info
                using (var command = new NpgsqlCommand("Select * from \"Presentations\"", conn))
                {
                    // Reads through the data base and load a Observer/Builder used for tracking presentations
                    DateTime dateAndTime;
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        dateAndTime = DateTime.Parse(reader["Time"].ToString()!);
                        if (dateAndTime.ToShortDateString() == DateTime.Now.ToShortDateString())
                        {
                            Builder.getBuilder().GetPresentation().room = reader["Room"].ToString().PadLeft(3, '0')!;
                            Builder.getBuilder().GetPresentation().rawDateTime = DateTime.Parse(reader["Time"].ToString()!);
                            Builder.getBuilder().GetPresentation().Major = reader["Major"].ToString().ToLower();
                            Builder.getBuilder().UpdateDateTime();
                            Builder.addToList();
                            Builder.getBuilder().NewPresentation();
                        }
                    }
                    // Closes database connection
                    //Console.WriteLine("Closing database connection...");
                    reader.Close();
                }
            }

        }
        // Basic database object that grabs the presentation room number and date/time
        // Used for testing - CKC
        public class DBData
        {
            public string? room { get; set; }
            public DateTime? rawDateTime { get; set; }
            public string? Mayor { get; set; }
        }

    }
}
