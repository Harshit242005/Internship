using Enhance.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;


namespace Enhance.Pages
{
    public class Group
    {
        public int Number { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public int Project { get; set; }
        public int Members { get; set; }
        public byte[] ImageData { get; set; }
        public bool IsApplied { get; set; }
    }

    public class GroupsModel : PageModel
    {
        private readonly ILogger<GroupsModel> _logger;
        private string connectString = "Server=localhost;Database=ZEUS;Uid=root;Pwd=azxcvbnmlkjhgfds;";

        public GroupsModel(ILogger<GroupsModel> logger)
        {
            _logger = logger;
            GetGroups = new List<Group>();
        }
        public List<int> GroupUserIds { get; set; }
        public List<Group> GetGroups { get; set; }
        public IActionResult OnGet()
        {
            ViewData["Page"] = "Show";
            GetGroups = new List<Group>();

            // Fetch the list of GroupId values for which the current user has applied
            List<int> appliedGroupIds = new List<int>();
            using (var connection = new MySqlConnection(connectString))
            {
                connection.Open();
                string appliedQuery = "SELECT GroupId FROM Applications WHERE UserId = @UserId";

                using (var appliedCommand = new MySqlCommand(appliedQuery, connection))
                {
                    appliedCommand.Parameters.AddWithValue("@UserId", LoginModel.UserId);

                    using (var appliedReader = appliedCommand.ExecuteReader())
                    {
                        while (appliedReader.Read())
                        {
                            int groupId = appliedReader.GetInt32("GroupId");
                            appliedGroupIds.Add(groupId);
                            
                        }
                    }
                }

                string groupsQuery = "SELECT g.Number, g.UserId, g.Name, g.Project, g.Members, u.Image " +
                                     "FROM Usersgroups g " +
                                     "JOIN Users u ON g.UserId = u.Id " +
                                     "ORDER BY g.Number DESC";

                using (var groupsCommand = new MySqlCommand(groupsQuery, connection))
                {
                    using (var groupsReader = groupsCommand.ExecuteReader())
                    {
                        while (groupsReader.Read())
                        {
                            Group group = new Group
                            {
                                Number = groupsReader.GetInt32("Number"),
                                UserId = groupsReader.GetInt32("UserId"),
                                Name = groupsReader.GetString("Name"),
                                Project = groupsReader.GetInt32("Project"),
                                Members = groupsReader.GetInt32("Members"),
                                ImageData = (byte[])groupsReader["Image"] // Assign user image data
                            };

                            // Set the IsApplied property based on whether the current group's UserId exists in the appliedGroupIds list
                            group.IsApplied = appliedGroupIds.Contains(group.UserId);
                            
                            GetGroups.Add(group);
                        }
                    }
                }
            }

            GroupUserIds = GetGroups.Select(g => g.UserId).ToList();
            

            return Page();
        }



        public IActionResult OnPost()
        {
            int groupId = int.Parse(Request.Form["groupId"]);
            int userId = int.Parse(Request.Form["userId"]);

            using (MySqlConnection connection1 = new MySqlConnection(connectString))
            {
                connection1.Open();
                string selectQuery = "SELECT UserId FROM Usersgroups WHERE UserId = @userId";

                using (MySqlCommand command1 = new MySqlCommand(selectQuery, connection1))
                {
                    command1.Parameters.AddWithValue("@userId", userId);

                    using (MySqlDataReader reader = command1.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // User exists in the table, perform the redirection
                            return RedirectToPage("/AlreadyExist");
                        }
                        
                    }
                }
            }

            // Replace with your actual connection string
            using (MySqlConnection connection = new MySqlConnection(connectString))
            {
                connection.Open();
                string query = "INSERT INTO Applications (UserId, GroupId) VALUES (@UserId, @GroupId)";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@GroupId", groupId);

                    command.ExecuteNonQuery();
                }

                string insertquery = "INSERT INTO MYgroup (GroupId, UserId) VALUES (@GroupId, @UserId)";

                using (MySqlCommand command = new MySqlCommand(insertquery, connection))
                {
                    command.Parameters.AddWithValue("@GroupId", groupId);
                    command.Parameters.AddWithValue("@UserId", userId);

                    command.ExecuteNonQuery();
                }
            }

            return RedirectToPage("/Cards");
        }







    }
}
