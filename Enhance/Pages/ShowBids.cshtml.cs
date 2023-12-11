using Enhance.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Collections.Generic; // Add this line


using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;



using System.Reflection.PortableExecutable;

namespace Enhance.Pages
{
    public class ShowBidsModel : PageModel
    {
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
                    message.Subject = "Project Approval Notification";

                    // Construct the email body using the project data
                    message.Body = new TextPart("plain")
                    {
                        Text = $"Your project '{emailData.ProjectName}' has been approved.\n" +
                               $"Project Description: {emailData.Description}\n" +
                               $"Duration: {emailData.Duration} {emailData.DurationType}\n" +
                               $"Post Date: {emailData.PostDate}\n" +
                               $"Price: {emailData.Price} {emailData.CurrencyType}\n" +
                               $"Id: {emailData.Id}"
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

        public List<BidInfo> AllBids { get; set; }
        public int Number { get; set; }
        public void OnGet()
        {
            ViewData["Page"] = "Show";
            Number = int.Parse(Request.Query["Number"]);
            string connectionString = "Server=localhost;Database=ZEUS;Uid=root;Pwd=azxcvbnmlkjhgfds;";
            string query = $"SELECT b.*, u.Image FROM BID b JOIN Users u ON b.Id = u.Id WHERE b.Project_Id = {Number}";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();

                AllBids = new List<BidInfo>(); // Initialize the AllBids list

                while (reader.Read())
                {
                    BidInfo bid = new BidInfo
                    {
                        Id = reader.GetInt32("Id"),
                        Number = reader.GetInt32("Number"),
                        Bid = reader.GetString("Bid"),
                        Description = reader.GetString("Description"),
                        Duration = reader.GetString("Duration"),
                        DurationType = reader.GetString("DurationType"),
                        ImageData = (byte[])reader["Image"]
                        // Add more properties from the table as needed
                    };

                    AllBids.Add(bid);
                }

                reader.Close();
            }
        }


        public IActionResult OnPost()
        {
            string connectionString = "server=localhost;user=root;password=azxcvbnmlkjhgfds;database=ZEUS"; // Replace with your actual connection string
            int number = int.Parse(Request.Form["Number"]);

            // this is a check
            if (number == 0)
            {
                return RedirectToPage("/Null");
            }
            MySqlConnection connection = new MySqlConnection(connectionString);



            // Step 1: Retrieve the Id from the BID table based on the Number
            string getIdQuery = "SELECT Project_ID, Id FROM BID WHERE Number = @Number";
            MySqlCommand command = new MySqlCommand(getIdQuery, connection);

            command.Parameters.AddWithValue("@Number", number);
            connection.Open();
            MySqlDataReader reader1 = command.ExecuteReader();
            int id = 0;
            int projectId = 0;
            if (reader1.Read())
            {
                projectId = reader1.GetInt32("Project_ID");
                id = reader1.GetInt32("Id");
            }

            reader1.Close();
            // Now, we will fetch the email address from the SIGNUP table
            string getEmailQuery = "SELECT Email FROM SIGNUP WHERE Id = @Id";
            using (MySqlCommand getEmailCommand = new MySqlCommand(getEmailQuery, connection))
            {
                getEmailCommand.Parameters.AddWithValue("@Id", id);
                string recipientEmail = getEmailCommand.ExecuteScalar().ToString();

                // Now, we will fetch the project data from the POST_PROJECT table
                string getProjectDataQuery = "SELECT ProjectName, Description, Duration, DurationType, PostDate, Price, CurrencyType " +
                                             "FROM POST_PROJECT WHERE Number = @ProjectId";
                using (MySqlCommand getProjectDataCommand = new MySqlCommand(getProjectDataQuery, connection))
                {
                    getProjectDataCommand.Parameters.AddWithValue("@ProjectId", projectId);
                    using (MySqlDataReader reader = getProjectDataCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Now we have the project data, so we can create the EmailData object
#pragma warning disable CS8601 // Possible null reference assignment.
#pragma warning disable CS8601 // Possible null reference assignment.
#pragma warning disable CS8601 // Possible null reference assignment.
#pragma warning disable CS8601 // Possible null reference assignment.
#pragma warning disable CS8601 // Possible null reference assignment.
                            EmailData emailData = new EmailData
                            {
                                ProjectName = reader["ProjectName"].ToString(),
                                Description = reader["Description"].ToString(),
                                Duration = Convert.ToInt32(reader["Duration"]),
                                DurationType = reader["DurationType"].ToString(),
                                PostDate = reader["PostDate"].ToString(),
                                Price = Convert.ToInt32(reader["Price"]),
                                CurrencyType = reader["CurrencyType"].ToString(),
                                Id = id
                            };
#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning restore CS8601 // Possible null reference assignment.

                            // Now we have both the recipient's email address and the project data
                            // We can send the email using the EmailSender class
                            EmailSender emailSender = new EmailSender();
                            emailSender.SendEmail(recipientEmail, emailData);

                            // Redirect to a success page or perform any other actions you need
                        }
                    }
                }


                // Step 2: Check the status of the project
                string checkStatusQuery = "SELECT Status FROM POST_PROJECT WHERE Number = @ProjectId";
                using (MySqlCommand statusCommand = new MySqlCommand(checkStatusQuery, connection))
                {
                    statusCommand.Parameters.AddWithValue("@ProjectId", projectId);
                    int status = Convert.ToInt32(statusCommand.ExecuteScalar());

                    if (status == 0)

                    {
                        string updateStatus = "UPDATE BID SET Status = 1 WHERE Number = @number";
                        using (MySqlCommand updateCommand = new MySqlCommand(updateStatus, connection))
                        {
                            updateCommand.Parameters.AddWithValue("@number", number);

                            updateCommand.ExecuteNonQuery();
                            // Query executed successfully
                        }

                        string updateAssigned = "UPDATE POST_PROJECT SET Assigned = @id WHERE Number = @ProjectId";
                        using (MySqlCommand updateCommand = new MySqlCommand(updateAssigned, connection))
                        {
                            updateCommand.Parameters.AddWithValue("@id", id);
                            updateCommand.Parameters.AddWithValue("@ProjectId", projectId);
                            updateCommand.ExecuteNonQuery();
                        }


                        // Step 3: Update the status to 1
                        string updateStatusQuery = "UPDATE POST_PROJECT SET Status = 1 WHERE Number = @ProjectId";
                        using (MySqlCommand updateCommand = new MySqlCommand(updateStatusQuery, connection))
                        {
                            updateCommand.Parameters.AddWithValue("@ProjectId", projectId);
                            updateCommand.ExecuteNonQuery();
                        }

                        // Step 4: Increase the Approved column value in the Users table with the same Project_id
                        string increaseApprovedQuery = "UPDATE Users SET Approved = Approved + 1 WHERE Number = @ProjectId";
                        using (MySqlCommand increaseCommand = new MySqlCommand(increaseApprovedQuery, connection))
                        {
                            increaseCommand.Parameters.AddWithValue("@ProjectId", projectId);
                            increaseCommand.ExecuteNonQuery();
                        }
                    }
                }
            }



            return RedirectToPage("/AllProjects");

        }
    }

    public class BidInfo
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string Bid { get; set; }
        public string Description { get; set; }
        public string Duration { get; set; }
        public string DurationType { get; set; }
        public byte[] ImageData { get; set; } // Image data of the user
        // Add more properties from the table as needed
    }
}
