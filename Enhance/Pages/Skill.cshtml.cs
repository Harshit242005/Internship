using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using MySql.Data.MySqlClient;


namespace Enhance.Pages
{
    public class SkillModel : PageModel
    {
        public List<SkillProject> Projects { get; set; }
        public static string skillName {  get; set; }
        public void OnGet()
        {
            ViewData["Page"] = "Show";
            skillName = Request.Query["skillName"];   
            
            string connectionString = "Server=localhost;Database=Skill;Uid=root;Pwd=azxcvbnmlkjhgfds;";
            string query = $"SELECT * FROM {skillName} WHERE Id = {LoginModel.UserId}";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();

                
                Projects = new List<SkillProject>();
                while (reader.Read())
                {
                    SkillProject project = new SkillProject
                    
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

    public class SkillProject
    {
        public int Number { get; set; }
        public string Project{ set; get; }
        public string Url { get; set; }
    }
}
