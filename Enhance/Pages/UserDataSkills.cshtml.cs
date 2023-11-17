using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Enhance.Pages
{
    public class UserDataSkillsModel : PageModel
    {
        public void OnGet()
        {
            ViewData["Page"] = "Show";
        }


        public int Id = LoginModel.UserId;

        [BindProperty(Name = "SkillName")]
        public string? SkillName { get; set; }

        

        private string connectionString = "server=localhost;database=ZEUS;user=root;password=azxcvbnmlkjhgfds";
        public IActionResult OnPost() 
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Skills (Id, SkillName) " +
                               "VALUES (@Id, @SkillName)";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", Id);  // Set User_Id based on the logged-in user's ID
                    command.Parameters.AddWithValue("@SkillName", SkillName);
                    command.ExecuteNonQuery();
                }
            }
            return RedirectToPage("/UserDataSkills");
        }
    }
}


