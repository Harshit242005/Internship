using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;
using System.Data;
using System;

namespace ProjectPro.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }
        public string Message { get; set; }
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            // Validate user input (you can add more validation logic)
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                Message = "Please provide both email and password.";
                return Page();
            }

            // Define your PostgreSQL connection string
            string connectionString = "Host=localhost;Port=5432;Database=projectpro;Username=postgres;Password=zxcvbnm;";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                // Prepare the SQL query
                string sql = "SELECT * FROM users WHERE email = @email";
                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@email", Email);

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string storedPasswordHash = reader["password"].ToString(); // Retrieve the stored password hash from the database
                            int userId = Convert.ToInt32(reader["id"]);
                            // Compare the input password with the stored hash (you may need to use a password hashing library here)
                            if (BCrypt.Net.BCrypt.Verify(Password, storedPasswordHash))
                            {
                                // Login successful
                                return RedirectToPage("/Home", new { Id = userId });
                            }
                        }
                    }
                }

                Message = "Invalid email or password.";
                return Page();
            }
        }
    }
}
