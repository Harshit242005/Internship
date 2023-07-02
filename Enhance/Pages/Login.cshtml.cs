using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace Enhance.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ILogger<LoginModel> _logger;
        public static int UserId { get; set; } // Public static variable to store the UserId

        public LoginModel(ILogger<LoginModel> logger)
        {
            _logger = logger;
        }

        private readonly string connectionString = "server=localhost;database=ZEUS;user=root;password=azxcvbnmlkjhgfds";

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public IActionResult OnPost()
        {
            string email = Email;
            string password = Password;

            // Establish a connection to the MySQL database
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Create a command to check if the email and password exist in the table
                string query = "SELECT COUNT(*) FROM SIGNUP WHERE Email = @Email AND Password = @Password";
                
                using (var command = new MySqlCommand(query, connection))
                {
                    // Set the parameter values
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password);

                    // Execute the query and retrieve the count of matching rows
                    int count = Convert.ToInt32(command.ExecuteScalar());

                    if (count > 0)
                    {
                        // Credentials are valid, retrieve the UserId
                        query = "SELECT id FROM SIGNUP WHERE Email = @Email AND Password = @Password";
                        using (var getUserIdCommand = new MySqlCommand(query, connection))
                        {
                            // Set the parameter values
                            getUserIdCommand.Parameters.AddWithValue("@Email", email);
                            getUserIdCommand.Parameters.AddWithValue("@Password", password);

                            // Execute the query and retrieve the UserId
                            var result = getUserIdCommand.ExecuteScalar();

                            if (result != null)
                            {
                                // Store the UserId in the static variable
                                UserId = Convert.ToInt32(result);

                                // this is for creating a session period and how we can login it's going to create a session period for us
                                // this will first run a select query in which it will check if the id already exist in the table or not and if it exist then 
                                // run the update or insert query on that behalf 
                                string selectQuery = "SELECT COUNT(*) FROM activity WHERE Id = @Id";

                                using (var selectCommand = new MySqlCommand(selectQuery, connection))
                                {
                                    // Set the parameter value
                                    selectCommand.Parameters.AddWithValue("@Id", UserId);

                                    // Execute the query and retrieve the count of matching rows
                                    int count1 = Convert.ToInt32(selectCommand.ExecuteScalar());

                                    if (count1 > 0)
                                    {
                                        // UserId already exists, update the lastActivity column
                                        string updateQuery = "UPDATE activity SET lastActivity = NOW() WHERE Id = @Id";

                                        using (var updateCommand = new MySqlCommand(updateQuery, connection))
                                        {
                                            // Set the parameter value
                                            updateCommand.Parameters.AddWithValue("@Id", UserId);

                                            // Execute the query to update the lastActivity column
                                            updateCommand.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        // UserId doesn't exist, insert a new row
                                        string insertQuery = "INSERT INTO activity (Id, lastActivity) VALUES (@Id, NOW())";

                                        using (var insertCommand = new MySqlCommand(insertQuery, connection))
                                        {
                                            // Set the parameter value
                                            insertCommand.Parameters.AddWithValue("@Id", UserId);

                                            // Execute the query to insert the data into the activity table
                                            insertCommand.ExecuteNonQuery();
                                        }
                                    }
                                }

                                // Redirect to a success page or perform other actions
                                return RedirectToPage("/Cards");
                            }
                        }
                    }
                    else
                    {
                        // Credentials are invalid, show an error message
                        ModelState.AddModelError(string.Empty, "Invalid email or password");
                    }
                }
            }

            // If the execution reaches here, it means the login was unsuccessful
            return Page();
        }
    }
}
