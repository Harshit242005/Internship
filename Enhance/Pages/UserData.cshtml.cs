using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.IO;

namespace Enhance.Pages
{
    public class UserDataModel : PageModel
    {
        public bool ShowSecondPage { get; set; }
        public IActionResult OnGet()
        {
            ViewData["Page"] = "Show";

            int userId = LoginModel.UserId;

            // Perform a database query to check if the user exists
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Users WHERE Id = @UserId";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);

                    int count = Convert.ToInt32(command.ExecuteScalar());
                    ShowSecondPage = (count > 0);
                }

                if (ShowSecondPage)
                {
                    return RedirectToPage("/ShowData");
                }
                return Page();
            }

            
        }
        public int Id = LoginModel.UserId;

        [BindProperty(Name = "Nickname")]
        public string Nickname { get; set; }

        [BindProperty(Name = "Dob")]
        public DateTime Dob { get; set; }

        [BindProperty(Name = "ImageUpload")]
        public IFormFile ImageUpload { get; set; }

        [BindProperty(Name = "Country")]
        public string Country { get; set; }

        [BindProperty(Name = "Contact")]
        public string Contact { get; set; }

        private string connectionString = "server=localhost;database=ZEUS;user=root;password=azxcvbnmlkjhgfds";


        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                if (ImageUpload != null && ImageUpload.Length > 0)
                {
                    
                    using (var connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();

                        // Get the file name
                        string fileName = ImageUpload.FileName;

                        // Convert the DateTime object to string in the desired format
                        string formattedDob = Dob.ToString("yyyy-MM-dd");

                        // Convert the uploaded image to a byte array
                        byte[] imageData;
                        using (var stream = new System.IO.MemoryStream())
                        {
                            ImageUpload.CopyTo(stream);
                            imageData = stream.ToArray();
                        }

                        // Prepare the SQL statement to insert the data into the table
                        string sql = "INSERT INTO Users (Id, Nickname, Dob, Image, Country, Contact) VALUES (@Id, @Nickname, @Dob, @Image, @Country, @Contact)";
                        using (var command = new MySqlCommand(sql, connection))
                        {
                            // Add parameters to the SQL command
                            command.Parameters.AddWithValue("@Id", Id);
                            command.Parameters.AddWithValue("@Nickname", Nickname);
                            command.Parameters.AddWithValue("@Dob", formattedDob);
                            command.Parameters.AddWithValue("@Image", imageData);
                            command.Parameters.AddWithValue("@Country", Country);
                            command.Parameters.AddWithValue("@Contact", Contact);

                            // Execute the SQL command
                            command.ExecuteNonQuery();
                        }

                        // Close the database connection
                        connection.Close();
                    }
                }

                // Optionally, you can redirect to another page after successful submission
                
            }
            return RedirectToPage("/UserDataSkills");
        }
    }
}
