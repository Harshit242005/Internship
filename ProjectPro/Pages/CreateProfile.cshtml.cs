using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;

namespace ProjectPro.Pages
{
    public class CreateProfileModel : PageModel
    {
        [FromQuery]
        public int Id { get; set; }

        [BindProperty]
        public string Nickname { get; set; }

        [BindProperty]
        public IFormFile ProfileImage { get; set; }

        [BindProperty]
        public string Position { get; set; }

        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            int userId = Id; // Retrieve Id from the query string
            if (ModelState.IsValid)
            {
                if (ProfileImage != null && ProfileImage.Length > 0)
                {
                    

                    // Create an NpgsqlConnection using the connection string
                    string connectionString = "Host=localhost;Port=5432;Database=projectpro;Username=postgres;Password=zxcvbnm;";
                    using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                    {
                        connection.Open();
                        string fileName = ProfileImage.FileName;

                        byte[] profileImageData;
                        using(var stream = new System.IO.MemoryStream())
                        {
                            ProfileImage.CopyTo(stream);
                            profileImageData = stream.ToArray();
                        }

                        string insertQuery = "INSERT INTO profile (id, nickname, image, position) VALUES (@id, @nickname, @image, @position)";
                        using (NpgsqlCommand command = new NpgsqlCommand(insertQuery, connection))
                        {
                            command.Parameters.AddWithValue("@id", Id);
                            command.Parameters.AddWithValue("@nickname", Nickname);
                            command.Parameters.AddWithValue("@image", profileImageData);
                            command.Parameters.AddWithValue("@position", Position);
                            int rowsAffected = command.ExecuteNonQuery();
                            connection.Close();
                        }
                    }

                }

            }
            return RedirectToPage("/Home", new { Id = userId });
        }
        



    }
}
