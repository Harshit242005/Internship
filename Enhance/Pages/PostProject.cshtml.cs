using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
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

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO POST_PROJECT (User_Id, ProjectName, Description, Duration, DurationType, PostDate, Price, CurrencyType) " +
                               "VALUES (@User_Id, @ProjectName, @Description, @Duration, @DurationType, TIMESTAMP(NOW()), @Price, @CurrencyType)";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@User_Id", Id);  // Set User_Id based on the logged-in user's ID
                    command.Parameters.AddWithValue("@ProjectName", ProjectName);
                    command.Parameters.AddWithValue("@Description", Description);
                    command.Parameters.AddWithValue("@Duration", Duration);
                    command.Parameters.AddWithValue("@DurationType", DurationType);
                    command.Parameters.AddWithValue("@Price", Price);
                    command.Parameters.AddWithValue("@CurrencyType", CurrencyType);
                    command.ExecuteNonQuery();
                }
            }
            return RedirectToPage("/Cards");
        }
    }
}
