using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
namespace Enhance.Pages
{
    public class ShowCidModel : PageModel
    {

        public int Number { get; set; }
        public string Cid;
        public int UserId { get; set; }
        public int Approve;

        public class EmailSender
        {
            private readonly string smtpServer = "smtp.gmail.com";
            private readonly int smtpPort = 587;
            private readonly string smtpUsername = "agreharshit610@gmail.com";
            private readonly string smtpPassword = "lbqxavlpxnewvczt";

            public void SendEmail(string recipientEmail)
            {
                try
                {
                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress("Sender Name", smtpUsername));
                    message.To.Add(new MailboxAddress("Recipient Name", recipientEmail));
                    message.Subject = "Project Accepted Notification";

                    // Construct the email body using the project data
                    message.Body = new TextPart("plain")
                    {
                        Text = "You're submitted CID has been accepted and project is finished"
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


        public void OnGet()
        {

            Number = int.Parse(Request.Query["Cid"]);

            string connectionString = "Server=localhost;Database=ZEUS;Uid=root;Pwd=azxcvbnmlkjhgfds;";
            string query = "SELECT Cid, UserId, Approve FROM CID WHERE ProjectId = @Number";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Number", Number);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Cid = reader.GetString("Cid");
                        UserId = reader.GetInt32("UserId");
                        Approve = reader.GetInt32("Approve");
                    }
                }

            }
        }

        // here will come all the details for the post method and how we will delete the data


        public IActionResult OnPost()
        {
            int CidNumber = int.Parse(Request.Form["CidNumber"]);
            int FinishedId = int.Parse(Request.Form["FinishedId"]);
            string connectionString = "Server=localhost;Database=ZEUS;Uid=root;Pwd=azxcvbnmlkjhgfds;";

            string UpdateFinished = "UPDATE Users SET Finished = Finished + 1 WHERE Id = @UserId";
            string UpdateApprove = "UPDATE CID SET Approve = 1 WHERE ProjectId = @Number";

            using (MySqlConnection connection1 = new MySqlConnection(connectionString))
            {
                connection1.Open();
                try
                {
                    // Execute the second query to update the Approve column in the CID table
                    string getEmailQuery = "SELECT Email FROM SIGNUP WHERE Id = @Id";
                    using (MySqlCommand getEmailCommand = new MySqlCommand(getEmailQuery, connection1))
                    {
                        getEmailCommand.Parameters.AddWithValue("@Id", FinishedId);
                        string recipientEmail = getEmailCommand.ExecuteScalar().ToString();

                        EmailSender emailSender = new EmailSender();
                        emailSender.SendEmail(recipientEmail);
                    }

                    using (MySqlCommand updateApproveCmd = new MySqlCommand(UpdateApprove, connection1))
                    {
                        updateApproveCmd.Parameters.AddWithValue("@Number", CidNumber);
                        updateApproveCmd.ExecuteNonQuery();
                    }

                    using (MySqlCommand updateApproveCmd = new MySqlCommand(UpdateFinished, connection1))
                    {
                        updateApproveCmd.Parameters.AddWithValue("@UserId", FinishedId);
                        updateApproveCmd.ExecuteNonQuery();
                    }
                }
                catch
                {
                    return RedirectToPage("/Error");
                }

                return RedirectToPage("/AllProjects");
            }
        }


    }
}
