using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;


using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Enhance.Pages.Shared;

namespace Enhance.Pages
{
    public class FreelancerModel : PageModel
    {
        // classes for email sending 
        public class EmailData
        {
            public string ProjectName { get; set; }
            public string Description { get; set; }
            public int Duration { get; set; }
            public string DurationType { get; set; }
            public string PostDate { get; set; }
            public int Price { get; set; }
            public string CurrencyType { get; set; }
            public int Id { get; set; }
        }


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
                    message.Subject = "Project Removed Notification";

                    // Construct the email body using the project data
                    message.Body = new TextPart("plain")
                    {
                        Text = "You have been removed from the project.\n" +
                               $"Your project '{emailData.ProjectName}' has been approved.\n" +
                               $"Project Description: {emailData.Description}\n" +
                               $"Duration: {emailData.Duration} {emailData.DurationType}\n" +
                               $"Post Date: {emailData.PostDate}\n" +
                               $"Price: {emailData.Price} {emailData.CurrencyType}\n"

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

        public int Id { get; set; }
        public string? Description { get; set; }
        public int Amount { get; set; }
        public string? Duration { get; set; }
        public string? DurationType { get; set; }
        public byte[] Image { get; set; }
        public string? Gmail { get; set; }
        public int ProjectNumber { get; set; }
        public void OnGet()
        {
            ViewData["Page"] = "Show";

            ProjectNumber = int.Parse(Request.Query["Project"]);
            // here we got the project number which you can use in the bid table to get the data of the bid where status = 1
            string connectionString = "Server=localhost;Database=ZEUS;Uid=root;Pwd=azxcvbnmlkjhgfds;";

            string GetFreelancer = "SELECT Id, Bid, Description, Duration, DurationType FROM BID Where Project_ID = @id AND Status = 1";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlCommand selectData = new MySqlCommand(GetFreelancer, connection))
                {
                    selectData.Parameters.AddWithValue("@id", ProjectNumber);

                    // read out the value 
                    using (MySqlDataReader reader = selectData.ExecuteReader())
                    {
                        if (reader.Read()) // Check if there's a row returned
                        {
                            // Read the values from the reader and store them in the variables
                            Id = reader.GetInt32("Id");
                            Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString("Description");
                            Amount = reader.GetInt32("Bid"); // Assuming the "Bid" column holds the "Amount" value
                            Duration = reader.IsDBNull(reader.GetOrdinal("Duration")) ? null : reader.GetString("Duration");
                            DurationType = reader.IsDBNull(reader.GetOrdinal("DurationType")) ? null : reader.GetString("DurationType");
                        }

                    }
                }

                // get the image of the freelancer from the Users table
                string getUsersQuery = "SELECT Image FROM Users WHERE Id = @id";
                using (MySqlCommand getUsersCmd = new MySqlCommand(getUsersQuery, connection))
                {
                    getUsersCmd.Parameters.AddWithValue("@id", Id);

                    using (MySqlDataReader userReader = getUsersCmd.ExecuteReader())
                    {
                        if (userReader.Read()) // Check if there's a row returned
                        {
                            // Read the image and other information from the reader and store them in the variables
                            Image = (byte[])userReader["Image"];
                        }
                    }
                }

                string getGmailQuery = "SELECT Email FROM SIGNUP WHERE Id = @id";
                using (MySqlCommand getGmailCmd = new MySqlCommand(getGmailQuery, connection))
                {
                    getGmailCmd.Parameters.AddWithValue("@id", Id);

                    using (MySqlDataReader gmailReader = getGmailCmd.ExecuteReader())
                    {
                        if (gmailReader.Read()) // Check if there's a row returned
                        {
                            // Read the email from the reader and store it in a variable
                            Gmail = gmailReader["Email"].ToString();
                            // Now you have the email address associated with the specified Id.
                        }
                    }
                }


            }



        }

        // here comes the post method
        public IActionResult OnPost()
        {
            int number = int.Parse(Request.Form["Number"]);
            string connectionString = "Server=localhost;Database=ZEUS;Uid=root;Pwd=azxcvbnmlkjhgfds;";

            // Update the Status and Assigned columns in the POST_PROJECT table
            string updateQuery = "UPDATE POST_PROJECT SET Status = 0, Assigned = 0 WHERE Number = @number";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection))
                {
                    updateCommand.Parameters.AddWithValue("@number", number);
                    // Execute the update query
                    int rowsAffected = updateCommand.ExecuteNonQuery();
                }

                string recipientEmail;
                string getEmailQuery = "SELECT Email FROM Signup WHERE Id = (SELECT ID FROM Bid WHERE Number = @projectNumber)";
           
                using (MySqlCommand getEmailCommand = new MySqlCommand(getEmailQuery, connection))
                {
                    getEmailCommand.Parameters.AddWithValue("@projectNumber", number);

                    using (MySqlDataReader reader = getEmailCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            recipientEmail = reader["Email"].ToString();

                            string getProjectDataQuery = "SELECT ProjectName, Description, Duration, DurationType, PostDate, Price, CurrencyType " +
                                                         "FROM POST_PROJECT WHERE Number = @ProjectId";
                            reader.Close();
                            using (MySqlCommand getProjectDataCommand = new MySqlCommand(getProjectDataQuery, connection))
                            {
                                getProjectDataCommand.Parameters.AddWithValue("@ProjectId", number);

                                using (MySqlDataReader projectDataReader = getProjectDataCommand.ExecuteReader())
                                {
                                    if (projectDataReader.Read())
                                    {
                                        // Now we have the project data, so we can create the EmailData object
                                        EmailData emailData = new EmailData
                                        {
                                            ProjectName = projectDataReader["ProjectName"].ToString(),
                                            Description = projectDataReader["Description"].ToString(),
                                            Duration = Convert.ToInt32(projectDataReader["Duration"]),
                                            DurationType = projectDataReader["DurationType"].ToString(),
                                            PostDate = projectDataReader["PostDate"].ToString(),
                                            Price = Convert.ToInt32(projectDataReader["Price"]),
                                            CurrencyType = projectDataReader["CurrencyType"].ToString(),
                                        };

                                        EmailSender emailSender = new EmailSender();
                                        emailSender.SendEmail(recipientEmail, emailData);
                                    }
                                }
                            }
                        }
                    }
                }

            }

            return RedirectToPage("/AllProjects");
        }
    }


}
