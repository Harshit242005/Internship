using Enhance.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Collections.Generic; // Add this line

namespace Enhance.Pages
{
    public class ShowBidsModel : PageModel
    {
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
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Step 1: Retrieve the Id from the BID table based on the Number
                string getIdQuery = "SELECT Project_ID, Id FROM BID WHERE Number = @Number";
                using (MySqlCommand command = new MySqlCommand(getIdQuery, connection))
                {
                    command.Parameters.AddWithValue("@Number", number);

                    int projectId = Convert.ToInt32(command.ExecuteScalar());
                    int id = Convert.ToInt32(command.ExecuteScalar());
                    if (projectId == 0)
                    {
                        return RedirectToPage("/Null");
                    }

                    if (id == 0)
                    {
                        return RedirectToPage("/Null");
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
