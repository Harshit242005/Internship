using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;


namespace Enhance.Pages
{
    public class UserDataEnrolled
    {
        public int Id { get; set; }
        public byte[]? Image { get; set; }
        public string? Nickname { get; set; }
        public int Finished { get; set; }
        public int Approved { get; set; }
        public string? Country { get; set; }
        public string? Dob { get; set; }
    }
    public class EnrolledGroupModel : PageModel
    {

        private readonly string connectionString = "Server=localhost;Database=ZEUS;Uid=root;Pwd=azxcvbnmlkjhgfds;";

        public List<UserData>? UserApplications { get; set; }

        public IActionResult OnGet()
        {
            ViewData["Page"] = "Show";
            UserApplications = FetchUserApplications();
            return Page();
        }

        private List<UserData> FetchUserApplications()
        {
            List<UserData> userApplications = new List<UserData>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                List<int> userIds = GetUserIdsFromMyGroup(connection);

                foreach (int userId in userIds)
                {
                    string selectUserQuery = $"SELECT Id, Image, Nickname, Finished, Approved, Country, Dob FROM Users WHERE Id = {userId}";

                    using (MySqlCommand selectUserCommand = new MySqlCommand(selectUserQuery, connection))
                    {
                        using (MySqlDataReader userReader = selectUserCommand.ExecuteReader())
                        {
                            while (userReader.Read())
                            {
                                UserData userApplication = new UserData
                                {
                                    Id = userReader.GetInt32("Id"),
                                    Image = (byte[])userReader["Image"],
                                    Nickname = userReader.GetString("Nickname"),
                                    Finished = userReader.GetInt32("Finished"),
                                    Approved = userReader.GetInt32("Approved"),
                                    Country = userReader.GetString("Country"),
                                    Dob = userReader.GetString("Dob")
                                };

                                userApplications.Add(userApplication);
                            }
                        }
                    }
                }
            }

            return userApplications;
        }

        private List<int> GetUserIdsFromMyGroup(MySqlConnection connection)
        {
            List<int> userIds = new List<int>();
            int userIdCheck = LoginModel.UserId;
            string selectUserIdsQuery = $"SELECT UserId FROM MYgroup WHERE GroupId = (SELECT GroupId FROM MYgroup WHERE UserId = {userIdCheck})";

            using (MySqlCommand selectUserIdsCommand = new MySqlCommand(selectUserIdsQuery, connection))
            {
                using (MySqlDataReader userReader = selectUserIdsCommand.ExecuteReader())
                {
                    while (userReader.Read())
                    {
                        int userId = userReader.GetInt32("UserId");
                        userIds.Add(userId);
                    }
                }
            }

            return userIds;
        }
    }
}
