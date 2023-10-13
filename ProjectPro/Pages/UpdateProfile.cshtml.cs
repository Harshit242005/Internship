using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;
using System.Globalization;

namespace ProjectPro.Pages
{
    public class UpdateProfileModel : PageModel
    {
        [BindProperty(SupportsGet = true)] // This attribute binds query parameters
        public int Id { get; set; }

        // data to be shown to himeself 
        public string? Name { get; set; }
        public string? position { get; set; }

        public byte[] Image { get; set; }

        // data for getting project in which user exist
        public int projectNumber { get; set; }
        public string? projectName { get; set; }
        public void OnGet()
        {
            int userId = Id;
            string connectionString = "Host=localhost;Port=5432;Database=projectpro;Username=postgres;Password=zxcvbnm;";
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                string getUserData = "SELECT nickname, image, position from profile where id = @id";
                using (NpgsqlCommand command = new NpgsqlCommand(getUserData, connection))
                {
                    command.Parameters.AddWithValue("@id", userId);
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Name = reader.GetString(0); // Assuming 'nickname' is at index 0
                            Image = (byte[])reader.GetValue(1); // Assuming 'image' is at index 1
                            position = reader.GetString(2); // Assuming 'position' is at index 2
                        }
                        reader.Close();
                    }

                }
                connection.Close();
            }

            // to get the project number in which if user in 
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                string getProjectNumber = "SELECT projectid FROM members WHERE @userId = ANY(member)";

                using (NpgsqlCommand command = new NpgsqlCommand(getProjectNumber, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            projectNumber = reader.GetInt32(0); // Assuming 'projectid' is at index 0
                        }
                        reader.Close();
                    }
                }
                connection.Close();
            }

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                string getName = "SELECT name from projects WHERE number = @number";
                using (NpgsqlCommand command = new NpgsqlCommand(getName, connection))
                {
                    command.Parameters.AddWithValue("@number", projectNumber);
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            projectName = reader.GetString(0); // Assuming 'name' is at index 0
                        }
                    }
                }
            }
        }

        public IActionResult OnPost()
        {
            int userId = Id;

            // we would remove the id from the project itself
            string connectionString = "Host=localhost;Port=5432;Database=projectpro;Username=postgres;Password=zxcvbnm;";
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                string removeId = "UPDATE members SET member = ARRAY_REMOVE(member, @userId) WHERE projectid = @projectId";
                using (NpgsqlCommand command = new NpgsqlCommand(removeId, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@projectId", projectNumber);

                    int rowsAffected = command.ExecuteNonQuery(); // Get the number of rows affected

                    if (rowsAffected > 0)
                    {
                        // If rows were affected, the user was successfully removed from the project.
                        return RedirectToPage("/Home", new { Id = userId });
                    }
                    else
                    {
                        // If no rows were affected, the removal was not successful, so return to the same page.
                        return RedirectToPage("/UpdateProfile", new { Id = userId });
                    }
                }
            }

        }
    }
}
