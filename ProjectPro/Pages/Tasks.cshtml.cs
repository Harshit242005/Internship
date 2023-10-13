using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;
using System.Data;
using System.Threading.Tasks;

namespace ProjectPro.Pages
{
    // class to handle the details for the users details
    public class Task
    {
        public int Number { get; set; }
        public DateTime Date { get; set; }
        public string? Description { get; set; }
        public int Status { get; set; }
        public string? Heading { get; set; }
    }

    public class TasksModel : PageModel
    {
        [BindProperty(SupportsGet = true)] // This attribute binds query parameters
        public int Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Helper { get; set; }
        [BindProperty(SupportsGet = true)]
        public int ProjectId { get; set; }

        public List<Task>? TaskDetails;
        public void OnGet()
        {
            TaskDetails = new List<Task>();
            string connectionString = "Host=localhost;Port=5432;Database=projectpro;Username=postgres;Password=zxcvbnm;";
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                string task = "SELECT number, heading, description, date, status from tasks where project = @project and helper = @helper";
                using (NpgsqlCommand command = new NpgsqlCommand(task, connection))
                {
                    command.Parameters.AddWithValue("@project", ProjectId);
                    command.Parameters.AddWithValue("@helper", Helper);

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Task taskdata = new Task
                            {
                                Number = reader.GetInt32(0),
                                Heading = reader.GetString(1),
                                Description = reader.GetString(2),
                                Date = reader.GetDateTime(3),
                                Status = reader.GetInt32(4)
                            };

                            TaskDetails.Add(taskdata);
                            
                        }
                    }
                }
            }
        }

        public IActionResult OnPost()
        {
            int myId = Id;
            int help = Helper;
            int project = ProjectId;

            int projectNumber = int.Parse(Request.Form["Number"]);
            string connectionString = "Host=localhost;Port=5432;Database=projectpro;Username=postgres;Password=zxcvbnm;";
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open(); // Open the connection
                string update = "UPDATE tasks SET status = 1 WHERE number = @number";
                using (NpgsqlCommand command = new NpgsqlCommand(update, connection))
                {
                    command.Parameters.AddWithValue("@number", projectNumber);
                    command.ExecuteNonQuery();
                }

                return RedirectToPage("/Tasks", new { Id = myId, Helper = help, ProjectId = project });
            }
        }
    }
}
