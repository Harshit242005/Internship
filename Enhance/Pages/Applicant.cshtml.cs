
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using MySql.Data.MySqlClient;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;


namespace Enhance.Pages
{
    public class EmailData1
    {
        public string? GroupName1 { get; set; }
        public string? Subject { get; set; }
    }
    // send a email to notify that member has been removed from the group
    public class EmailSender1
    {
        private readonly string smtpServer = "smtp.gmail.com";
        private readonly int smtpPort = 587;
        private readonly string smtpUsername = "agreharshit610@gmail.com";
        private readonly string smtpPassword = "lbqxavlpxnewvczt";

        public void SendEmail(string recipientEmail, EmailData1 emailData1)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Sender Name", smtpUsername));
                message.To.Add(new MailboxAddress("Recipient Name", recipientEmail));
                message.Subject = $"{emailData1.Subject}";

                // Construct the email body using the project data
                message.Body = new TextPart("plain")
                {
                    Text = $"You application for {emailData1.GroupName1} has not been accepted"
                };

                using (var client = new SmtpClient())
                {
                    client.Connect(smtpServer, smtpPort, SecureSocketOptions.StartTls);
                    client.Authenticate(smtpUsername, smtpPassword);
                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
            }
        }
    }


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


                // this query will update the lastActivity column of the activity column
                string updateQuery1 = "UPDATE activity SET lastActivity = NOW() WHERE Id = @Id";

                using (var updateCommand = new MySqlCommand(updateQuery1, connection))
                {

                    // Set the parameter value
                    updateCommand.Parameters.AddWithValue("@Id", LoginModel.UserId);

                    // Execute the query to update the lastActivity column
                    updateCommand.ExecuteNonQuery();
                }

                return RedirectToPage("/Groups");
            }

            else
            {
                return RedirectToPage("/ErrorType");
            }


        }








        public IActionResult OnPostDelete()
        {
            string connectionString = "Server=localhost;Database=ZEUS;Uid=root;Pwd=azxcvbnmlkjhgfds;";

            // Retrieve the applicantId and groupId from the form
            if (int.TryParse(Request.Form["applicantId"], out int applicantId) &&
                int.TryParse(Request.Form["groupId"], out int groupId))
            {
                // Delete the applicant with the given ID from the Applications table
                // where UsersId matches the applicantId

                using var connection = new MySqlConnection(connectionString);
                connection.Open();

                var deleteQuery = "DELETE FROM Applications WHERE UserId = @applicantId";
                using var deleteCommand = new MySqlCommand(deleteQuery, connection);
                deleteCommand.Parameters.AddWithValue("@applicantId", applicantId);
                deleteCommand.ExecuteNonQuery();

                // Send an email notification to the recipient using the EmailSender
                string recipientEmail = GetRecipientEmail(connection, applicantId);
                string groupName = GetGroupName(connection, groupId);

                EmailData1 emailData = new EmailData1
                {
                    GroupName1 = groupName,
                    Subject = "Application not been accepted"
                };

                EmailSender1 emailSender = new EmailSender1();
                emailSender.SendEmail(recipientEmail, emailData);
            }

            // Redirect to the page
            return RedirectToPage("/Groups");
        }

        // Helper method to get recipient email from Signup table
        private string GetRecipientEmail(MySqlConnection connection, int applicantId)
        {
            string getEmailQuery = "SELECT Email FROM Signup WHERE Id = @applicantId";
            string recipientEmail = string.Empty;
            using (var getEmailCommand = new MySqlCommand(getEmailQuery, connection))
            {
                getEmailCommand.Parameters.AddWithValue("@applicantId", applicantId);
                using (var reader = getEmailCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        recipientEmail = reader.GetString("Email");
                    }
                }
            }
            return recipientEmail;
        }

        // Helper method to get group name from usersgroups table
        private string GetGroupName(MySqlConnection connection, int groupId)
        {
            string getGroupNameQuery = "SELECT Name FROM usersgroups WHERE UserId = @groupId";
            string groupName = string.Empty;
            using (var getGroupNameCommand = new MySqlCommand(getGroupNameQuery, connection))
            {
                getGroupNameCommand.Parameters.AddWithValue("@groupId", groupId);
                using (var reader = getGroupNameCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        groupName = reader.GetString("Name");
                    }
                }
            }
            return groupName;
        }


        public override bool Equals(object? obj)
        {
            return obj is ApplicantModel model &&
                   EqualityComparer<ILogger<ApplicantModel>>.Default.Equals(_logger, model._logger);
        }
    }

}
