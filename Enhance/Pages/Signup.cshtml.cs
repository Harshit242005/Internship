using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


using MySql.Data.MySqlClient;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;


namespace Enhance.Pages
{
    public class SignupModel : PageModel
    {
        public void OnGet()
        {
        }

        private readonly string connectionString = "server=localhost;database=ZEUS;user=root;password=azxcvbnmlkjhgfds";
        
        [BindProperty]
        public string Email { get; set; }

        [BindProperty] 
        public string Password { get; set; }

        public static string code;
        public class EmailSender
        {
            private readonly string smtpServer = "smtp.gmail.com";
            private readonly int smtpPort = 587;
            private readonly string smtpUsername = "agreharshit610@gmail.com";
            private readonly string smtpPassword = "lbqxavlpxnewvczt";

            public void SendEmail(string Email, string code)
            {
                try
                {
                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress("Sender Name", smtpUsername));
                    message.To.Add(new MailboxAddress("Recipient Name", Email));
                    message.Subject = "Verification code";
                    message.Body = new TextPart("plain")
                    {
                        Text = $"Your verification code is {code}"
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

        public IActionResult OnPost()
        {
            
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand("INSERT INTO SIGNUP (Email, Password, Created_at) VALUES (@Email, @Password, @Created_at)", connection))
                    {
                        command.Parameters.AddWithValue("@Email", Email);
                        command.Parameters.AddWithValue("@Password", Password);
                        command.Parameters.AddWithValue("@Created_at", DateTime.Now);

                        command.ExecuteNonQuery();
                    }
                }
                Random random = new Random();
                code = random.Next(100000, 999999).ToString();
                
                EmailSender emailSender = new EmailSender();
                emailSender.SendEmail(Email, code);

                return RedirectToPage("/Privacy", new { code = code });
            }
            catch (Exception ex)
            {
                return RedirectToPage("Error");
            }
        }
    }
}
