using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;
using Enhance.Pages.Shared;
using MailKit.Security;
using MimeKit;
using static Enhance.Pages.ShowBidsModel;



using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;


namespace Enhance.Pages
{
    public class CloseModel : PageModel
    {
        [BindProperty(Name = "Cid")]
        public string Cid { get; set; }
        // here is the input value of the cid by the user

        // for getting the email notification when user upload rhe cid 
        public class EmailData
        {
            public string? Cid { get; set; }
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
                    message.Subject = "Project has been finished";

                    // Construct the email body using the project data
                    message.Body = new TextPart("plain")
                    {
                        Text = $"Th project cid is {emailData.Cid}"
                          
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


        private readonly string _connectionString;
        public void OnGet()
        {
            ViewData["Page"] = "Show";
        }

        public CloseModel()
        {
            _connectionString = "server=localhost;user=root;password=azxcvbnmlkjhgfds;database=ZEUS";
        }


        public IActionResult OnPost()
        {
            int ProjectId = int.Parse(Request.Query["Project"]);
            int User = LoginModel.UserId;


            string queryCheckIfExists = "SELECT COUNT(*) FROM CID WHERE ProjectId = @ProjectId";
            string queryInsert = "INSERT INTO CID (ProjectId, Cid, UserId) VALUES (@ProjectId, @Cid, @UserId)";
            string queryUpdate = "UPDATE CID SET UserId = @UserId, Cid = @Cid WHERE ProjectId = @ProjectId";
            
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string getUserEmailQuery = "SELECT Email FROM SIGNUP WHERE Id = (SELECT User_Id FROM POST_PROJECT WHERE Number = @ProjectId)";
                using (MySqlCommand getUserEmailCmd = new MySqlCommand(getUserEmailQuery, connection))
                {
                    getUserEmailCmd.Parameters.AddWithValue("@ProjectId", ProjectId);
                    string recipientEmail = getUserEmailCmd.ExecuteScalar()?.ToString();

                    if (!string.IsNullOrEmpty(recipientEmail))
                    {
                        // Now we have the recipient's email address and the CID value
                        // We can send the email using the EmailSender class
                        EmailData emailData = new EmailData
                        {
                            // You can set the CID value here
                            Cid = Cid
                        };

                        EmailSender emailSender = new EmailSender();
                        emailSender.SendEmail(recipientEmail, emailData);
                    }
                }

                using (MySqlCommand cmdCheckIfExists = new MySqlCommand(queryCheckIfExists, connection))
                {
                    cmdCheckIfExists.Parameters.AddWithValue("@ProjectId", ProjectId);

                    int count = Convert.ToInt32(cmdCheckIfExists.ExecuteScalar());

                    if (count > 0) // Cid already exists, update the row
                    {
                        using (MySqlCommand cmdUpdate = new MySqlCommand(queryUpdate, connection))
                        {
                            cmdUpdate.Parameters.AddWithValue("@ProjectId", ProjectId);
                            cmdUpdate.Parameters.AddWithValue("@UserId", User);
                            cmdUpdate.Parameters.AddWithValue("@Cid", Cid);
                            cmdUpdate.ExecuteNonQuery();
                        }
                    }
                    else // Cid doesn't exist, insert a new row
                    {
                        using (MySqlCommand cmdInsert = new MySqlCommand(queryInsert, connection))
                        {
                            cmdInsert.Parameters.AddWithValue("@ProjectId", ProjectId);
                            cmdInsert.Parameters.AddWithValue("@Cid", Cid);
                            cmdInsert.Parameters.AddWithValue("@UserId", User);
                            cmdInsert.ExecuteNonQuery();
                        }
                    }
                }

                string updateQuery = "UPDATE POST_PROJECT SET `Check` = 1 WHERE Number = @ProjectId";
                using (MySqlCommand updatePostProjectCmd = new MySqlCommand(updateQuery, connection))
                {
                    updatePostProjectCmd.Parameters.AddWithValue("@ProjectId", ProjectId);
                    updatePostProjectCmd.ExecuteNonQuery();
                }
            }

            return RedirectToPage("/Cards");
        }
    }
}
