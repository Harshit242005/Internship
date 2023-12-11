using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using MySql.Data.MySqlClient;


namespace Enhance.Pages.Shared
{
    public class Project
    {
        public int? UserId { get; set; }
        public int? Number { get; set; }
        public string? ProjectName { get; set; }
        public string? Description { get; set; }
        public string? Duration { get; set; }
        public string? DurationType { get; set; }
        public string? PostDate { get; set; }
        public int? Price { get; set; }
        public string? CurrencyType { get; set; }
        public byte[] ImageData { get; set; } // New property to store user image data
        public int Check { get; set; }
    }

    public class ActiveProject
    {
        public int UserId { get; set; }
        public bool Active { get; set; }
    }

    public class CardsModel : PageModel
    {
        private readonly ILogger<CardsModel> _logger;
        private string connectString = "Server=localhost;Database=ZEUS;Uid=root;Pwd=azxcvbnmlkjhgfds;";

        // now i have to access tese 
        public List<ActiveProject> ActiveProjects { get; set; }

        // Fetch all the Id values from the Users table
        List<int> userIds = new List<int>();
        // this is the end of the variable 


        public CardsModel(ILogger<CardsModel> logger)
        {
            _logger = logger;
            ActiveProjects = new List<ActiveProject>();
        }

        public List<Project> Projects { get; set; }


        public void OnGet()
        {
            ViewData["Page"] = "Show";
            Projects = new List<Project>();

            using (var connection = new MySqlConnection(connectString))
            {
                connection.Open();

                string query = "SELECT p.Number, p.Check, p.User_Id, p.ProjectName, p.Description, p.Duration, p.DurationType, p.PostDate, p.Price, p.CurrencyType, u.Image " +
               "FROM POST_PROJECT p " +
               "JOIN Users u ON p.User_Id = u.Id " +
               "ORDER BY p.Number DESC";


                using (var command = new MySqlCommand(query, connection))
                {

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Project project = new Project
                            {
                                Number = reader.GetInt32("Number"),
                                UserId = reader.GetInt32("User_Id"),
                                ProjectName = reader.GetString("ProjectName"),
                                Description = reader.GetString("Description"),
                                Duration = reader.GetString("Duration"),
                                DurationType = reader.GetString("DurationType"),
                                PostDate = reader.GetString("PostDate"),
                                Price = reader.GetInt32("Price"),
                                CurrencyType = reader.GetString("CurrencyType"),
                                ImageData = (byte[])reader["Image"], // Assign user image data
                                Check = reader.GetInt32("Check")
                            };



                            Projects.Add(project);
                        }
                    }


                }

                // here we will execute the select query to fill out the value of the Active variable in the class 


                // Create a list to store ActiveProject instances


                using (var connection1 = new MySqlConnection(connectString))
                {
                    connection1.Open();

                    // Fetch the Id values from the Users table
                    string userIdQuery = "SELECT Id FROM Users";
                    using (var userIdCommand = new MySqlCommand(userIdQuery, connection1))
                    {
                        using (var userIdReader = userIdCommand.ExecuteReader())
                        {
                            while (userIdReader.Read())
                            {
                                int userId = userIdReader.GetInt32("Id");
                                userIds.Add(userId);
                            }
                        }
                    }

                    // Fetch the lastActivity column from the Activity table
                    string activityQuery = "SELECT Id, lastActivity FROM Activity";
                    using (var activityCommand = new MySqlCommand(activityQuery, connection1))
                    {
                        using (var activityReader = activityCommand.ExecuteReader())
                        {
                            string debugInfo = string.Empty;
                            while (activityReader.Read())
                            {
                                int userId = activityReader.GetInt32("Id");
                                DateTime lastActivity = TimeZoneInfo.ConvertTimeToUtc(activityReader.GetDateTime("lastActivity"));

                                // Calculate the time difference between lastActivity and current timestamp
                                DateTime currentTimestamp = DateTime.UtcNow;
                                TimeSpan timeDifference = currentTimestamp - lastActivity;

                                // Determine the Active value based on the time difference
                                bool isActive = timeDifference.TotalHours <= 1;

                                debugInfo += "lastActivity: " + lastActivity.ToString() + "<br>";
                                debugInfo += "currentTimestamp: " + currentTimestamp.ToString() + "<br>";
                                debugInfo += "timeDifference: " + timeDifference.ToString() + "<br>";
                                debugInfo += "isActive variable value: " + isActive.ToString() + "<br>";

                                // Create an instance of ActiveProject and set the UserId and Active properties
                                ActiveProject activeProject = new ActiveProject
                                {
                                    UserId = userId,
                                    Active = isActive
                                };

                                debugInfo += "Collected value is: " + activeProject.UserId + "project card value is: " + activeProject.Active + "<br>";

                                if (activeProject != null)
                                {
                                    ActiveProjects.Add(activeProject);
                                }
                                else
                                {
                                    RedirectToPage("/Null");
                                }

                                ViewData["DebugInfo"] = debugInfo;

                            }
                        }
                    }
                }
            }
        }


    }
}