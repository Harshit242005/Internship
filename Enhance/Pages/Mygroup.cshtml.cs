using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MimeKit;
using MySql.Data.MySqlClient;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Enhance.Pages
{
    public class UserData
    {
        public int Id { get; set; }
        public byte[] Image { get; set; }
        public string Nickname { get; set; }
        public int Finished { get; set; }
        public int Approved { get; set; }
        public string Country { get; set; }
        public string Dob { get; set; }
    }

    public class EmailData
    {
        public string GroupName { get; set; }

    }
    // send a email to notify that member has been removed from the group
    public class EmailSender
    {
        private readonly string smtpServer = "smtp.gmail.com";
        private readonly int smtpPort = 587;
        private readonly string smtpUsername = "agreharshit610@gmail.com";
        private readonly string smtpPassword = "lbqxavlpxnewvczt";

        public void SendEmail(string recipientEmail, EmailData emailData)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Sender Name", smtpUsername));
                message.To.Add(new MailboxAddress("Recipient Name", recipientEmail));
                message.Subject = "Group Removal Notification";

                // Construct the email body using the project data
                message.Body = new TextPart("plain")
                {
                    Text = $"Tou have been removed from the group {emailData.GroupName}"
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

    public class MygroupModel : PageModel
    {
        private readonly string connectionString = "Server=localhost;Database=ZEUS;Uid=root;Pwd=azxcvbnmlkjhgfds;";

        public List<UserData> UserApplications { get; set; }

        


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

            string selectUserIdsQuery = $"SELECT UserId FROM MYgroup WHERE GroupId = {LoginModel.UserId}";

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

        public IActionResult OnPost()
        {
            int Id = int.Parse(Request.Form["Id"]);

            // Create a MySqlConnection object
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                // Open the database connection
                connection.Open();

                // Create a MySqlCommand object with the delete query
                string deleteQuery = "DELETE FROM MYgroup WHERE UserId = @Id";
                using (MySqlCommand command = new MySqlCommand(deleteQuery, connection))
                {
                    // Set the parameter value for Id
                    command.Parameters.AddWithValue("@Id", Id);

                    // Execute the delete query
                    command.ExecuteNonQuery();
                }

                // for sending off the email to the users


                // Step 2: Get the email of the recipient from the Signup table
                string getEmailQuery = "SELECT Email FROM Signup WHERE Id = @UserDeleteId";
                string recipientEmail = string.Empty;
                using (MySqlCommand getEmailCommand = new MySqlCommand(getEmailQuery, connection))
                {
                    // Set the parameter value for UserDeleteId
                    getEmailCommand.Parameters.AddWithValue("@UserDeleteId", Id);

                    // Execute the query and read the result
                    using (MySqlDataReader reader = getEmailCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            recipientEmail = reader.GetString("Email");
                        }

                    }
                }

                // Step 3: Get the name of the group from usersgroups table
                string getGroupNameQuery = "SELECT Name FROM usersgroups WHERE UserId = @UserId";
                string groupName = string.Empty;
                using (MySqlCommand getGroupNameCommand = new MySqlCommand(getGroupNameQuery, connection))
                {
                    // Set the parameter value for UserId
                    getGroupNameCommand.Parameters.AddWithValue("@UserId", LoginModel.UserId);

                    // Execute the query and read the result
                    using (MySqlDataReader reader = getGroupNameCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            groupName = reader.GetString("Name");
                        }

                    }
                }

                // Now that you have the recipient's email and the group name, you can send the email notification
                // Assuming you have an instance of EmailSender class called "emailSender"
                EmailData emailData = new EmailData { GroupName = groupName };
                EmailSender emailSender = new EmailSender();
                emailSender.SendEmail(recipientEmail, emailData);



                // query to update the members of the group
                string updateQuery = "UPDATE usersgroups SET Members = Members - 1 WHERE UserId = @Id";

                using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
                {
                    // Set the parameter value for Id
                    command.Parameters.AddWithValue("@Id", LoginModel.UserId);

                    // Execute the update query
                    command.ExecuteNonQuery();
                }

                string updateProject = "UPDATE usersgroups SET Project = Project - (SELECT Finished FROM Users WHERE Id = @Id) WHERE UserId = @GroupUserId";
                using (MySqlCommand command = new MySqlCommand(updateProject, connection))
                {
                    command.Parameters.AddWithValue("@Id", Id);
                    command.Parameters.AddWithValue("@GroupUserId", LoginModel.UserId);
                    command.ExecuteNonQuery();
                }
            }
            return RedirectToPage("/MYgroup");
        }

    }
}

