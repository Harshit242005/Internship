using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;


namespace ProjectPro.Pages
{
    public class TaskModel : PageModel
    {
        [BindProperty(SupportsGet = true)] // This attribute binds query parameters
        public int Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public int AssistantId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Number { get; set; }


        // Now we need to create some variable to hold some data for the onget method 
        // getting the project name 
        public string? ProjectName { get; set; }
        // getting the users image to whom the task is going to assign
        public string? Base64Image { get; set; }

        // variables to hold the incoming data for the form 
        [BindProperty]
        public string? Heading { get; set; }

        [BindProperty]
        public string? Description { get; set; }
        public void OnGet()
        {
            

            // connection with database and tables
            string connectionString = "Host=localhost;Port=5432;Database=projectpro;Username=postgres;Password=zxcvbnm;";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                // Query to fetch the image data based on the user's ID
                string selectQuery = "SELECT name FROM projects WHERE number = @number";

                using (NpgsqlCommand command = new NpgsqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@number", Number);
                    ProjectName = command.ExecuteScalar() as string;
                }
                connection.Close();
            }


            // to get the image data with the id 
            byte[] imageData;

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                // Query to fetch the image data based on the user's ID
                string selectQuery = "SELECT image FROM profile WHERE id = @id";

                using (NpgsqlCommand command = new NpgsqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@id", AssistantId);
                    imageData = (byte[])command.ExecuteScalar();
                }

                connection.Close();
            }

            if (imageData != null)
            {
                // Convert the image data to a Base64 string
                string base64Image = Convert.ToBase64String(imageData);
                Base64Image = base64Image;
            }
            else
            {
                Base64Image = null;
            }

        }


        public IActionResult OnPost()
        {
            // Your connection string
            string connectionString = "Host=localhost;Port=5432;Database=projectpro;Username=postgres;Password=zxcvbnm;";
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // Construct the SQL INSERT statement
                    string insertQuery = "INSERT INTO tasks (Project, Heading, Description, Owner, Helper, status, date) " +
                                         "VALUES (@Project, @Heading, @Description, @Owner, @Helper, 0, current_date)";

                    using (var command = new NpgsqlCommand(insertQuery, connection))
                    {
                        // Bind the parameters
                        command.Parameters.AddWithValue("@Project", Number);
                        command.Parameters.AddWithValue("@Heading", Heading);
                        command.Parameters.AddWithValue("@Description", Description);
                        command.Parameters.AddWithValue("@Owner", Id);
                        command.Parameters.AddWithValue("@Helper", AssistantId);

                        // Execute the INSERT query
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // Insertion was successful
                            return RedirectToPage("/Home", new { Id });
                        }
                        else
                        {
                            // Insertion failed, handle accordingly
                            TempData["ErrorMessage"] = "Failed to create the task.";
                            return Page(); // Return to the current page or handle the error as needed.
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during database operations
                TempData["ErrorMessage"] = "An error occurred: " + ex.Message;
                return Page(); // Return to the current page or handle the error as needed.
            }
        }

    }
}
