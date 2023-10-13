using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;

namespace ProjectPro.Pages
{
    public class TaskDetailsModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Project { get; set; }

        // task details
        public string? Heading { get; set; }
        public string? Description { get; set; }

        public void OnGet()
        {
            string connectionString = "Host=localhost;Port=5432;Database=projectpro;Username=postgres;Password=zxcvbnm;";
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                string selectQuery = "SELECT heading, description FROM tasks WHERE helper = @Id AND date = current_date;";

                using (NpgsqlCommand command = new NpgsqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", Id);

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Heading = reader.GetString(0);
                            Description = reader.GetString(1);
                        }
                    }
                }
            }
        }
    }
}
