using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Enhance.Pages
{
    public class AllProjectsModel : PageModel
    {
        public List<Project> Projects { get; set; }

        public void OnGet()
        {
            ViewData["Page"] = "Show";
            Projects = new List<Project>();

            string connectionString = "Server=localhost;Database=ZEUS;Uid=root;Pwd=azxcvbnmlkjhgfds;";
            string query = $@"
        SELECT p.*,
               (SELECT COUNT(*) FROM BID b WHERE b.Project_ID = p.Number) AS Bid
        FROM POST_PROJECT p
        WHERE p.User_Id = {LoginModel.UserId}";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Project project = new Project
                    {
                        Name = reader.GetString("ProjectName"),
                        Number = reader.GetInt32("Number"),
                        Price = reader.GetInt32("Price"),
                        Type = reader.GetString("CurrencyType"),
                        PostDate = reader.GetDateTime("PostDate").ToString("yyyy-MM-dd"),
                        Status = reader.GetInt32("Status"),
                        Bid = reader.GetInt32("Bid") // Read the bid count from the subquery
                    };

                    Projects.Add(project);
                }

                reader.Close(); // Close the reader after the while loop
            }


            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                foreach (Project project in Projects)
                {
                    string cidQuery = $"SELECT * FROM CID WHERE ProjectId = {project.Number} AND Cid IS NOT NULL";
                    MySqlCommand cidCommand = new MySqlCommand(cidQuery, connection);
                    MySqlDataReader cidReader = cidCommand.ExecuteReader();

                    if (cidReader.HasRows)
                    {
                        project.Status = 2;
                    }

                    cidReader.Close();
                }

            }
        }

        public IActionResult OnPost()
        {
            int number = int.Parse(Request.Form["Number"]);

            string connectionString = "Server=localhost;Database=ZEUS;Uid=root;Pwd=azxcvbnmlkjhgfds;";
            string query = $"DELETE FROM POST_PROJECT WHERE Number = {number}";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }

            return Redirect("/AllProjects"); // or return any other appropriate response
        }
    }

    public class Project
    {
        public string Name { get; set; }
        public int Number { get; set; }
        public int Price { get; set; }
        public string Type { get; set; }
        public string PostDate { get; set; }
        public int Bid { get; set; } // New property to store the row count
        public int Status { get; set; }
        // Add more properties from the table as needed
    }
}
