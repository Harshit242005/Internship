using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;


namespace Enhance.Pages
{
    public class SkillProjectModel : PageModel
    {
        [BindProperty(Name = "id")]
        public int Id { get; set; }

        [BindProperty(Name ="Project")]
        public string Project { get; set; }

        [BindProperty(Name ="Url")]


        public string Url { get; set; }
        public string skillName = SkillModel.skillName;
        public void OnGet()
        {
            ViewData["Page"] = "Show";
            
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (string.IsNullOrEmpty(skillName))
            {
                ModelState.AddModelError(string.Empty, "Invalid skill name.");
                return Page();
            }

            int userId = LoginModel.UserId;
            string connectionString = "Server=localhost;Database=Skill;Uid=root;Pwd=azxcvbnmlkjhgfds;";

            string query = $"INSERT INTO `{skillName}` (Id, Project, Url) VALUES (@Id, @Project, @Url)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", userId);
                command.Parameters.AddWithValue("@Project", Project);
                command.Parameters.AddWithValue("@Url", Url);

                command.ExecuteNonQuery();
            }

            return RedirectToPage("/ShowData");
        }
    }
}

