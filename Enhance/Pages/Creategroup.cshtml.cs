
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using MySql.Data.MySqlClient;
namespace Enhance.Pages
{
    public class CreategroupModel : PageModel
    {
        [BindProperty(Name = "Name")]
        public string Name { get; set; }
        public int Id = LoginModel.UserId;
        public int Finished { get; set; }
        public void OnGet()
        {
            ViewData["Page"] = "Show";

            string connectionString = "Server=localhost;Database=ZEUS;Uid=root;Pwd=azxcvbnmlkjhgfds;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT Finished FROM Users WHERE Id = @Id";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", Id);
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    Finished = reader.GetInt32("Finished");
                    // this will set the finished variable with the person who's creating the user 
                }

                reader.Close();
            }
        }

        public IActionResult OnPost()
        {
            string connectionString = "Server=localhost;Database=ZEUS;Uid=root;Pwd=azxcvbnmlkjhgfds;";
            string query = "INSERT INTO Usersgroups (UserId, Name, Project, Members) VALUES (@Id, @Name, (SELECT Finished FROM Users WHERE Id = @Id), @Members)";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", Id);  // Assign the value of Id to UserId
                command.Parameters.AddWithValue("@Name", Name);
                command.Parameters.AddWithValue("@Members", 1);

                command.ExecuteNonQuery();
            }

            string updateQuery = "UPDATE activity SET lastActivity = NOW() WHERE Id = @Id";
            using (MySqlConnection connection1 = new MySqlConnection(connectionString))
            using (var updateCommand = new MySqlCommand(updateQuery, connection1))
            {
                connection1.Open();
                // Set the parameter value
                updateCommand.Parameters.AddWithValue("@Id", Id);

                // Execute the query to update the lastActivity column
                updateCommand.ExecuteNonQuery();
            }

            return RedirectToPage("/Groups");
        }
    }

}
