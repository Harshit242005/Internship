using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;


namespace Enhance.Pages
{
    public class ShowDataModel : PageModel
    {
        public class Skill
        {
            public string SkillName;
        }
        public string Nickname { get; set; }
        public string Dob { get; set; }
        public string Country { get; set; }
        public string Contact { get; set; }
        public List<Skill> Skills { get; set; }
        public byte[] ImageData { get; set; }
        public int Finished { get; set; }
        public int Approved { get; set; }
        public string? Email { get; set; }
        public void OnGet()
        {
            ViewData["Page"] = "Show";

            string connectionString = "server=localhost;database=ZEUS;user=root;password=azxcvbnmlkjhgfds";
            int userId = LoginModel.UserId; // Provide the actual user ID or modify it accordingly
  
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string imageQuery = "SELECT Image FROM Users WHERE Id = @Id";


                using (var imageCommand = new MySqlCommand(imageQuery, connection))
                {
                    imageCommand.Parameters.AddWithValue("@Id", userId);
                    using (var reader = imageCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            ImageData = (byte[])reader["Image"];
                        }
                    }
                }
                
            }

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT Nickname, Dob, Country, Contact, Finished, Approved FROM Users WHERE Id = @Id";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", userId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Nickname = reader.GetString("Nickname");
                            Dob = reader.GetString("Dob");
                            Country = reader.GetString("Country");
                            Contact = reader.GetString("Contact");
                            Finished = reader.GetInt32("Finished");
                            Approved = reader.GetInt32("Approved");
                        }
                    }
                }
            }

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string getEmailQuery = "SELECT Email FROM SIGNUP WHERE Id = @Id";
                using (var command = new MySqlCommand(getEmailQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", userId);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Email = reader.GetString("Email");
                        }
                    }
                }
            }

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT SkillName FROM Skills WHERE Id = @Id";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", userId);

                    using (var reader = command.ExecuteReader())
                    {
                        Skills = new List<Skill>();

                        while (reader.Read())
                        {
                            string skillName = reader.GetString("SkillName");
                            Skills.Add(new Skill { SkillName = skillName });
                        }
                    }
                }
            }
        }
    }
}
