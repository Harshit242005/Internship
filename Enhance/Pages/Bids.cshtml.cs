using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Enhance.Pages
{
    public class BidsModel : PageModel
    {
        public class Bided_Project
        {
            public int IdValue { get; set; }
            public string Bid { get; set; }
            public string Duration { get; set; }
            public string DurationType { get; set; }
            public int Number { get; set; }
            public int Status { get; set; }
        }

        public int Id = LoginModel.UserId;
        private string connectString = "Server=localhost;Database=ZEUS;Uid=root;Pwd=azxcvbnmlkjhgfds;";
        public List<Bided_Project>? BidedProjects { get; set; }

        public void OnGet()
        {
            ViewData["Page"] = "Show";
            BidedProjects = new List<Bided_Project>();

            using (var connection = new MySqlConnection(connectString))
            {
                connection.Open();

                string query = "SELECT Id, Project_Id, Bid, Duration, Status, DurationType FROM BID WHERE Id = @Id";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", Id);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            BidedProjects.Add(new Bided_Project
                            {
                                IdValue = reader.GetInt32("Id"),
                                Number = reader.GetInt32("Project_Id"),
                                Bid = reader.GetString("Bid"),
                                Duration = reader.GetString("Duration"),
                                DurationType = reader.GetString("DurationType"),
                                Status = reader.GetInt32("Status")
                            });
                        }
                    }
                }
            }
        }
    }
}
