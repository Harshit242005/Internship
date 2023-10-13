using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;


namespace ProjectPro.Pages
{
    public class SignupModel : PageModel
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

            // Hash the password using BCrypt
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(Password);

            // Define your PostgreSQL connection string
            string connectionString = "Host=localhost;Port=5432;Database=projectpro;Username=postgres;Password=zxcvbnm;";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                // Prepare the SQL query to insert a new user with the hashed password
                string sql = "INSERT INTO users (email, password) VALUES (@email, @password)";
                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@email", Email);
                    command.Parameters.AddWithValue("@password", hashedPassword); // Insert the hashed password

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Message = "Signup successful. You can now log in.";
                        return RedirectToPage("/Login");
                    }
                    else
                    {
                        Message = "Signup failed. Please try again.";
                        return Page();
                    }
                }
            }
        }

    }
}
