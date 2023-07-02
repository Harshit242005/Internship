using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;


namespace Enhance.Pages
{
    public class UserProjectDetailsModel : PageModel
    {

        private readonly string _connectionString;

        public int Number { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public string DurationType { get; set; }
        public UserProjectDetailsModel()
        {
            _connectionString = "server=localhost;user=root;password=azxcvbnmlkjhgfds;database=ZEUS";
        }


        public void OnGet()
        {
            ViewData["Page"] = "Show";
            int Number = int.Parse(Request.Query["Number"]);

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                string query = "SELECT * FROM POST_PROJECT WHERE Number = @Number";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Number", Number);
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                // Use the retrieved data as needed
                if (reader.Read())
                {
                    // Retrieve the values of the columns from the row
                    Number = reader.GetInt32("Number");
                    ProjectName = reader.GetString("ProjectName");
                    Description = reader.GetString("Description");
                    DurationType = reader.GetString("DurationType");
                    Duration = reader.GetInt32("Duration");
                    // etc.
                }

                reader.Close();
            }
        }
    }
}
