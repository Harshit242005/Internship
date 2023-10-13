using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;

namespace ProjectPro.Pages
{
    public class CreateProjectModel : PageModel
    {
        [BindProperty]
        public string ProjectName { get; set; }

        [BindProperty]
        public DateTime StartDate { get; set; }

        [BindProperty]
        public DateTime EndDate { get; set; }

        // two variable of the bool class to check if anyhow the user exist in the group or own the group
        public bool checkOwner { get; set; }
        

        [FromQuery]
        public int Id { get; set; }
        public int projectId { get; set; }
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            int userId = Id; // Retrieve Id from the query string

            // Create an NpgsqlConnection using the connection string
            string connectionString = "Host=localhost;Port=5432;Database=projectpro;Username=postgres;Password=zxcvbnm;";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                string checkQuery = "SELECT Count(*) FROM members WHERE @userId = ANY(member)";
                using (NpgsqlCommand command = new NpgsqlCommand(checkQuery, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);

                    // ExecuteScalar returns the count of rows where id matches the userId
                    int rowCount = Convert.ToInt32(command.ExecuteScalar());

                    // If rowCount is greater than 0, it means there's at least one matching row
                    checkOwner = rowCount > 0;
                }
            }




            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open(); // Open the database connection

                // Example code to insert into the projects table:
                string projectInsertSql = "INSERT INTO projects (id, name, startdate, enddate) VALUES (@id, @name, @startdate, @enddate) RETURNING number";
                int projectNumber; // This will store the project number

                using (NpgsqlCommand projectInsertCommand = new NpgsqlCommand(projectInsertSql, connection))
                {
                    projectInsertCommand.Parameters.AddWithValue("@id", userId);
                    projectInsertCommand.Parameters.AddWithValue("@name", ProjectName);
                    projectInsertCommand.Parameters.AddWithValue("@startdate", StartDate);
                    projectInsertCommand.Parameters.AddWithValue("@enddate", EndDate);

                    // Execute the insert command and retrieve the project number
                    projectNumber = (int)projectInsertCommand.ExecuteScalar();
                }

                // Insert the project number and user Id into the members table
                string membersInsertSql = "INSERT INTO members (projectid, member) VALUES (@projectid, @member)";
                using (NpgsqlCommand membersInsertCommand = new NpgsqlCommand(membersInsertSql, connection))
                {
                    membersInsertCommand.Parameters.AddWithValue("@projectid", projectNumber);
                    membersInsertCommand.Parameters.AddWithValue("@member", new int[] { userId });

                    int rowsAffected = membersInsertCommand.ExecuteNonQuery();

                    // Check if the insertion was successful and handle accordingly
                    if (rowsAffected > 0)
                    {
                        // Insertion was successful
                        // You can redirect to a success page or perform other actions
                        return RedirectToPage("/Home", new { Id = userId });
                    }
                    else
                    {
                        // Insertion failed
                        // You can display an error message or handle the failure
                        return Page();
                    }
                }
            }
        }


    }
}
