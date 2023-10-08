using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;

namespace ProjectPro.Pages
{
    // class to handle the data of the profile table data 
    public class MemeberData
    {
        public int MemberId { get; set; }
        public byte[]? Image { get; set; }
        public string? Position { get; set; }
        public string? Nickname { get; set; }
        public bool check { get; set; }

    }



    public class CreateTaskModel : PageModel
    {
        public List<MemeberData> Members { get; set; } = new List<MemeberData>();
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        public int projectNumber { get; set; }

        public void OnGet()
        {

            string connectionString = "Host=localhost;Port=5432;Database=projectpro;Username=postgres;Password=zxcvbnm;";
            //  we have to get the member of the project and their data with two class 
            //  a list to handle the memeber id in the projects 
            //  but we have to get the latest project which is active and not closed 
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                string getProjectNumber = "SELECT number FROM projects where id = @id AND status = 0";
                using (NpgsqlCommand command = new NpgsqlCommand(getProjectNumber, connection))
                {
                    command.Parameters.AddWithValue("id", Id);
                    projectNumber = Convert.ToInt32(command.ExecuteScalar());
                }
                connection.Close();
            }

            // here we would create a list to store the id of the project excluding the founder ID 
            // Create a list to hold member IDs
            List<int> memberIds = new List<int>();
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open(); // Open the database connection

                // Define your query to retrieve member IDs based on projectNumber
                string getMemberIdsQuery = "SELECT member FROM members WHERE projectid = @projectNumber";

                using (NpgsqlCommand command = new NpgsqlCommand(getMemberIdsQuery, connection))
                {
                    command.Parameters.AddWithValue("@projectNumber", projectNumber); // Set the projectNumber parameter

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Get the integer array from the reader and add its values to the memberIds list
                            if (!reader.IsDBNull(0))
                            {
                                int[] memberArray = reader.GetFieldValue<int[]>(0);
                                memberIds.AddRange(memberArray);
                            }
                        }
                    }
                }
            }
            // Remove the Id variable from the memberIds list if it exists
            memberIds.Remove(Id);

            // now we would run a for loop for each id stored in the list to get the data of that from the profile table 
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open(); // Open the database connection

                // Define your query to retrieve member data from the "profile" table for each member ID
                string getMemberDataQuery = "SELECT id, nickname, image, position FROM profile WHERE id = @memberId";

                foreach (int memberId in memberIds)
                {
                    using (NpgsqlCommand command = new NpgsqlCommand(getMemberDataQuery, connection))
                    {
                        command.Parameters.AddWithValue("@memberId", memberId); // Set the memberId parameter

                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Create a MemeberData object and populate it with data from the reader
                                MemeberData memberData = new MemeberData
                                {
                                    MemberId = reader.GetInt32(0),
                                    Nickname = reader.GetString(1),
                                    Image = reader.IsDBNull(2) ? null : (byte[])reader[2],
                                    Position = reader.GetString(3)
                                };

                                bool CheckIfTaskExists(int memberId)
                                {
                                    using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                                    {
                                        connection.Open();

                                        using (NpgsqlCommand command = new NpgsqlCommand())
                                        {
                                            command.Connection = connection;
                                            command.CommandText = "SELECT EXISTS (SELECT 1 FROM tasks WHERE date = current_date AND helper = @MemberId)";
                                            command.Parameters.AddWithValue("@MemberId", memberId);

                                            // Execute the query and return the result as a boolean.
                                            return (bool)command.ExecuteScalar();
                                        }
                                    }
                                }

                                bool taskExists = CheckIfTaskExists(memberData.MemberId);

                                // Set the check property based on whether the task exists
                                memberData.check = taskExists;

                                // Add the memberData object to the Members list
                                Members.Add(memberData);


                            }
                        }
                    }
                }
            }

            // Now, the Members list contains the data for each member ID in memberIds


        }

        
    }
}
