using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;

namespace ProjectPro.Pages
{
    // class to handle the projects details
    public class Projects
    {
        public int Number { get; set; }
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime StartDate { get; set; }
        // Add more properties as needed
        public DateTime EndDate { get; set; }
        public int Hash { get; set; }
    }
    public class CompletedModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        public List<Projects>? UserProjects { get; set; }
        public void OnGet()
        {
            string connectionString = "Host=localhost;Port=5432;Database=projectpro;Username=postgres;Password=zxcvbnm;";
            int userId = Id;
            UserProjects = new List<Projects>(); // Create a list to store project data

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                // Query to fetch project data based on the user's ID
                string selectQuery = "SELECT number, id, name, startdate, enddate, hash FROM projects WHERE id = @id";

                using (NpgsqlCommand command = new NpgsqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@id", userId);

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Projects project = new Projects
                            {
                                Number = reader.GetInt32(0),
                                Id = reader.GetInt32(1),
                                Name = reader.GetString(2),
                                StartDate = reader.GetDateTime(3),
                                EndDate = reader.GetDateTime(4),
                                Hash = reader.GetInt32(5),
                            };

                            UserProjects.Add(project);
                        }
                    }
                }

                connection.Close();
            }

        }
    }
}
