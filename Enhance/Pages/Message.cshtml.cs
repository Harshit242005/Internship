using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Enhance.Pages
{
    public class Username
    {
        public int Id { get; set; }
        public string Nickname { get; set; }
        public byte[] ImageData { get; set; }
    }

    public class MessageModel : PageModel
    {
        private string connectString = "Server=localhost;Database=ZEUS;Uid=root;Pwd=azxcvbnmlkjhgfds;";
        public List<Username> Usernames { get; set; } = new List<Username>();

        [BindProperty(Name = "message")]
        public string Message { get; set; }
        public int Id = LoginModel.UserId;
        public IActionResult OnGet()
        {
            // Use the 'id' value for further processing or storing it in a property
            // ...

            ViewData["Page"] = "Show";
            Usernames = new List<Username>();
            
            using (var connection = new MySqlConnection(connectString))
            {
                connection.Open();

                string query = $"SELECT u.Id, u.Nickname, u.Image " +
                               $"FROM Users u " +
                               $"WHERE u.Id <> {Id} " +
                               $"ORDER BY u.Id ASC";

                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Usernames.Add(new Username
                            {
                                Id = reader.GetInt32("Id"),
                                Nickname = reader.GetString("Nickname"),
                                ImageData = (byte[])reader["Image"]
                            });
                        }
                    }
                }
            }

            return Page();
        }
    }
}
