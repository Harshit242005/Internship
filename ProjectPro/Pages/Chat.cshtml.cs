

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;


namespace ProjectPro.Pages
{
    public class Chatproject
    {
        public int ProjectId { get; set; }
        public string? Name { get; set; }
        public int ProjectNumber { get; set; }
    }
    public class ChatModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        public bool idExistsInProfile;
        public string? Base64Image { get; set; }

        // we need three variables data to get to replace the value and the button's collection
        public int ProjectId { get; set; }
        public int ProjectNumber { get; set; }
        public string? Name { get; set; }

        public List<Chatproject>? UserProjects { get; set; }
        public void OnGet()
        {
            int userId = Id;
            /* to get the image data we woukd call another function */
            byte[] imageData;
            string connectionString = "Host=localhost;Port=5432;Database=projectpro;Username=postgres;Password=zxcvbnm;";
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                // Query to fetch the image data based on the user's ID
                string selectQuery = "SELECT image FROM profile WHERE id = @id";

                using (NpgsqlCommand command = new NpgsqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@id", userId);
                    imageData = (byte[])command.ExecuteScalar();
                }

                connection.Close();
            }

            // Convert the image data to a Base64 string
            string base64Image = Convert.ToBase64String(imageData);
            Base64Image = base64Image;

            //// here we would start filling the data as the one lowest person
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                string getProjectNumber = "SELECT projectid FROM members WHERE @id = ANY(member)";
                using (NpgsqlCommand command = new NpgsqlCommand(getProjectNumber, connection))
                {
                    command.Parameters.AddWithValue("@id", Id);
                    // ExecuteScalar to retrieve the projectNumber
                    ProjectNumber = (int)command.ExecuteScalar();
                }
            }

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                string getProjectInfo = "SELECT id, name FROM projects WHERE number = @ProjectNumber";
                using (NpgsqlCommand command = new NpgsqlCommand(getProjectInfo, connection))
                {
                    command.Parameters.AddWithValue("@ProjectNumber", ProjectNumber);
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                      {
                            ProjectId = reader.GetInt32(0); // Assuming 'id' is at index 0
                            Name = reader.GetString(1); // Assuming 'name' is at index 1
                        }
                    }
                }
            }






            UserProjects = new List<Chatproject>();
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                // Query to fetch project data based on the user's ID
                string selectQuery = "SELECT number, id, name FROM projects WHERE id = @id";

                using (NpgsqlCommand command = new NpgsqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@id", ProjectId);

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Chatproject projectData = new Chatproject
                            {
                                ProjectNumber = reader.GetInt32(0),
                                ProjectId = reader.GetInt32(1),
                                Name = reader.GetString(2),

                            };

                            UserProjects.Add(projectData);
                        }
                    }
                }
            }



        }
    }
}
