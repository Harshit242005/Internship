using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Org.BouncyCastle.Bcpg;

namespace Enhance.Pages
{
    public class PostProjectModel : PageModel
    {
        public void OnGet()
        {
            ViewData["Page"] = "Show";
        }
        

        public int Id = LoginModel.UserId;
        
        [BindProperty]
        public string? ProjectName { get; set; }

        [BindProperty]
        public string? Description { get; set; }

        [BindProperty]
        public int? Duration { get; set; }

        [BindProperty]
        public string? DurationType { get; set; }

        [BindProperty]
        public int? Price { get; set; }

        [BindProperty]
        public string? CurrencyType { get; set; }


        
        private string connectionString = new MySqlConnectionStringBuilder
        {
            Server = "localhost",
            Database = "ZEUS",
            UserID = "root",
            Password = "azxcvbnmlkjhgfds",
            SslMode = MySql.Data.MySqlClient.MySqlSslMode.Disabled
        }.ConnectionString;
        public IActionResult OnPost()
        {
            List<string> selectedTechnologies = new List<string>();
            foreach (var key in Request.Form.Keys)
            {
                if (key.StartsWith("technologies") && Request.Form.TryGetValue(key, out var value))
                {
                    selectedTechnologies.Add(value);
                }
            }

            // Serialize the selected technologies to JSON
            string skillRequiredJson = JsonConvert.SerializeObject(selectedTechnologies);

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO POST_PROJECT (User_Id, ProjectName, Description, Duration, DurationType, PostDate, Price, CurrencyType, skill_required) " +
                               "VALUES (@User_Id, @ProjectName, @Description, @Duration, @DurationType, TIMESTAMP(NOW()), @Price, @CurrencyType, @SkillRequired)";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@User_Id", Id);  // Set User_Id based on the logged-in user's ID
                    command.Parameters.AddWithValue("@ProjectName", ProjectName);
                    command.Parameters.AddWithValue("@Description", Description);
                    command.Parameters.AddWithValue("@Duration", Duration);
                    command.Parameters.AddWithValue("@DurationType", DurationType);
                    command.Parameters.AddWithValue("@Price", Price);
                    command.Parameters.AddWithValue("@CurrencyType", CurrencyType);
                    command.Parameters.AddWithValue("@SkillRequired", skillRequiredJson);
                    command.ExecuteNonQuery();
                }

                // now we will update the code in which we will run a update query
                string updateQuery = "UPDATE activity SET lastActivity = NOW() WHERE Id = @Id";

                using (var updateCommand = new MySqlCommand(updateQuery, connection))
                {
                    // Set the parameter value
                    updateCommand.Parameters.AddWithValue("@Id", Id);

                    // Execute the query to update the lastActivity column
                    updateCommand.ExecuteNonQuery();
                }
            }
            return RedirectToPage("/Cards");
        }
    }
}
