using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace Enhance.Pages
{
    public class SkillProjectProfile
    {
        public int Number { get; set; }
        public string Project { set; get; }
        public string Url { get; set; }
    }
    public class ProfileSkillsModel : PageModel
    {
        public List<SkillProjectProfile> Projects { get; set; }
        public static string skillName { get; set; }
        public void OnGet()
        {
            ViewData["Page"] = "Show";
#pragma warning disable CS8601 // Possible null reference assignment.
            skillName = Request.Query["skillName"];
#pragma warning restore CS8601 // Possible null reference assignment.
            int Id = int.Parse(Request.Query["Id"]);
            string connectionString = "Server=localhost;Database=Skill;Uid=root;Pwd=azxcvbnmlkjhgfds;";
            string query = $"SELECT * FROM {skillName} WHERE Id = {Id}";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();


                Projects = new List<SkillProjectProfile>();
                while (reader.Read())
                {
                    SkillProjectProfile project = new SkillProjectProfile

                    {
                        Number = reader.GetInt32("Number"),
                        Url = reader.GetString("Url"),
                        Project = reader.GetString("Project")
                        // Add more properties from the table as needed
                    };
                    Projects.Add(project);

                }

                reader.Close();
            }
        }
    }


}
