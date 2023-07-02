using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Text.RegularExpressions;


namespace Enhance.Pages
{
    public class UserApplicationData
    {
        public string Nickname { get; set; }
        public byte[] Image { get; set; }
        public int Finished { get; set; }
        public int Approved { get; set; }

        public int Id { get; set; }
    }
    public class ApplicantModel : PageModel
    {

        private readonly ILogger<ApplicantModel> _logger;

        public ApplicantModel(ILogger<ApplicantModel> logger)
        {
            _logger = logger;
        }
        public int groupId;
        public int MemberCount;
        public void OnGet()
        {
            ViewData["Page"] = "Show";

            if (int.TryParse(Request.Query["GroupId"], out int parsedGroupId))
            {
                groupId = parsedGroupId;
            }
            string connectionString = "Server=localhost;Database=ZEUS;Uid=root;Pwd=azxcvbnmlkjhgfds;";
            string query = $"SELECT UserId FROM Applications WHERE GroupId = {groupId}";
            string queryMember = $"SELECT Members FROM Usersgroups WHERE UserId = {groupId}";
            List<int> userIds = new();

            MySqlConnection mySqlConnection = new(connectionString);
            using MySqlConnection connection = mySqlConnection;
            connection.Open();

            var command = new MySqlCommand(query, connection);
            MySqlCommand command_1 = new(queryMember, connection);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int userId = reader.GetInt32("UserId");
                    userIds.Add(userId);
                }
            }

            using (MySqlDataReader reader = command_1.ExecuteReader())
            {
                while (reader.Read())
                {
                    MemberCount = reader.GetInt32("Members");
                }
            }

            List<UserApplicationData> userApplications = new();

            foreach (int userId in userIds)
            {
                string userQuery = $"SELECT Nickname, Id, Image, Finished, Approved FROM Users WHERE Id = {userId}";

                using MySqlCommand userCommand = new(userQuery, connection);
                MySqlDataReader mySqlDataReader = userCommand.ExecuteReader();
                using MySqlDataReader userReader = mySqlDataReader;
                while (userReader.Read())
                {
                    UserApplicationData userApplication = new()
                    {
                        Nickname = userReader.GetString("Nickname"),
                        Image = (byte[])userReader["Image"],
                        Finished = userReader.GetInt32("Finished"),
                        Approved = userReader.GetInt32("Approved"),
                        Id = userReader.GetInt32("Id")
                    };

                    userApplications.Add(userApplication);
                }
            }

            ViewData["UserApplications"] = userApplications;
        }

        public IActionResult OnPostAccept()
        {
            string connectionString = "Server=localhost;Database=ZEUS;Uid=root;Pwd=azxcvbnmlkjhgfds;";
            int applicantId = int.Parse(Request.Form["applicantId"]);
            int group = int.Parse(Request.Form["groupId"]);

            if (applicantId == 0 || group == 0) 
            {
                return RedirectToPage("/Null");
            }


            try
            {
                using var connection = new MySqlConnection(connectionString);
                connection.Open();

                var updateQuery = "UPDATE Usersgroups SET Project = Project + (SELECT Finished FROM Users WHERE Id = @applicantId), Members = Members + 1 WHERE UserId = @groupId";

                using var command = new MySqlCommand(updateQuery, connection);
                command.Parameters.AddWithValue("@applicantId", applicantId);
                command.Parameters.AddWithValue("@groupId", group);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)

                {

                    var insertQuery = "INSERT INTO mygroup (GroupId, UserId) VALUES (@groupId, @applicantId)";
                    using (MySqlCommand insertCommand = new(insertQuery, connection))
                    {
                        insertCommand.Parameters.AddWithValue("@groupId", group);
                        insertCommand.Parameters.AddWithValue("@applicantId", applicantId);
                        insertCommand.ExecuteNonQuery();
                    }


                    var deleteQuery = "DELETE FROM Applications WHERE UserId = @applicantId";
                    using (var deleteCommand = new MySqlCommand(deleteQuery, connection))
                    {
                        deleteCommand.Parameters.AddWithValue("@applicantId", applicantId);
                        deleteCommand.ExecuteNonQuery();
                    }

                    return RedirectToPage("/Cards");
                }

                else
                {
                    return RedirectToPage("/ErrorType");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                TempData["StackTrace"] = ex.StackTrace;
                return RedirectToPage("/Error");
            }
        }








        public IActionResult OnPostDelete()
        {
            string connectionString = "Server=localhost;Database=ZEUS;Uid=root;Pwd=azxcvbnmlkjhgfds;";
            // Retrieve the applicantId from the form
            if (int.TryParse(Request.Form["applicantId"], out int applicantId))
            {
                // Delete the applicant with the given ID from the Applications table
                // where UsersId matches the applicantId

                using var connection = new MySqlConnection(connectionString);
                connection.Open();

                var query = "DELETE FROM Applications WHERE UserId = @applicantId";

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@applicantId", applicantId);
                command.ExecuteNonQuery();
            }

            // Redirect to the page
            return RedirectToPage("/Groups");
        }

        public override bool Equals(object? obj)
        {
            return obj is ApplicantModel model &&
                   EqualityComparer<ILogger<ApplicantModel>>.Default.Equals(_logger, model._logger);
        }
    }

}
