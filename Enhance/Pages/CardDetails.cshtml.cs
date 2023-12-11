using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Diagnostics.Metrics;
using Newtonsoft.Json;

namespace Enhance.Pages
{
    public class CardDetailsModel : PageModel
    {
        private readonly string _connectionString;

        public int Id = LoginModel.UserId;
        public int Number { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public string DurationType { get; set; }
        public string PostDate { get; set; }
        public int UserId { get; set; }
        public bool Exist { get; set; }
        public int Status { get; set; }
        public List<string> SkillRequired { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public CardDetailsModel()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            _connectionString = "server=localhost;user=root;password=azxcvbnmlkjhgfds;database=ZEUS";
        }

        public void OnGet()
        {
            ViewData["Page"] = "Show";
            Number = int.Parse(Request.Query["Number"]);
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                string query = "SELECT ProjectName, Description, DurationType, Duration, PostDate, User_Id, Status, skill_required FROM POST_PROJECT WHERE Number = @Number";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Number", Number);
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    ProjectName = reader.GetString("ProjectName");
                    Description = reader.GetString("Description");
                    DurationType = reader.GetString("DurationType");
                    Duration = reader.GetInt32("Duration");
                    PostDate = reader.GetString("PostDate");

                    // Check if the value is NULL before trying to read it
                    if (!reader.IsDBNull(reader.GetOrdinal("User_Id")))
                    {
                        UserId = reader.GetInt32("User_Id");
                    }
                    else
                    {
                        // Handle the case where User_Id is NULL (set UserId to an appropriate default value)
                        UserId = 0; // Or any other default value
                    }

                    Status = reader.GetInt32("Status");

                    // Check if the value is NULL before trying to read it
                    if (!reader.IsDBNull(reader.GetOrdinal("skill_required")))
                    {
                        string skillRequiredJson = reader.GetString("skill_required");

                        // Deserialize JSON data into a list of strings
#pragma warning disable CS8601 // Possible null reference assignment.
                        SkillRequired = JsonConvert.DeserializeObject<List<string>>(skillRequiredJson);
#pragma warning restore CS8601 // Possible null reference assignment.
                    }
                    else
                    {
                        // Handle the case where skill_required is NULL (set SkillRequired to an appropriate default value)
                        SkillRequired = new List<string>(); // Or any other default value
                    }
                }



                reader.Close();
                int Project_ID = int.Parse(Request.Query["Number"]);
                query = "SELECT COUNT(*) FROM BID WHERE Id = @Id AND Project_ID = @ProjectId";
                command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", Id);
                command.Parameters.AddWithValue("@ProjectId", Project_ID);


                long result = (long)command.ExecuteScalar();
                int count = Convert.ToInt32(result);
                Exist = (count > 0);
                //  this query will update the query when the user click on get details button and update the user with login id last activty 
                string updateQuery = "UPDATE activity SET lastActivity = NOW() WHERE Id = @Id";
                command = new MySqlCommand(updateQuery, connection);
                command.Parameters.AddWithValue("@Id", Id);
                command.ExecuteNonQuery();
            }

            // there would be a check from the bid table and then we would find out what happens

        }

        public IActionResult OnPost()
        {
            string bid = Request.Form["Bid"];
            string description = Request.Form["Description"];
            string duration = Request.Form["Duration"];
            string durationType = Request.Form["DurationType"];
            int project_Id = int.Parse(Request.Form["Project_Id"]);
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                string query = "INSERT INTO BID (Id, Bid, Description, Duration, DurationType, Project_Id) VALUES (@Id, @Bid, @Description, @Duration, @DurationType, @Project_Id)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", Id);
                command.Parameters.AddWithValue("@Bid", bid);
                command.Parameters.AddWithValue("@Description", description);
                command.Parameters.AddWithValue("@Duration", duration);
                command.Parameters.AddWithValue("@DurationType", durationType);
                command.Parameters.AddWithValue("@Project_Id", project_Id); // Use UserId as Project_Id

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }

            return Redirect("/Cards");



        }
    }

}
