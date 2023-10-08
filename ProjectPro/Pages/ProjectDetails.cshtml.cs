using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;

namespace ProjectPro.Pages
{
    // class to store the data of the other members
    public class ProfileMembersData
    {
        public int Id { get; set; }
        public string? MemberNickname { get; set; }
        public byte[]? MemberImage { get; set; }
        public string? MemberPosition { get; set; }
    }
    public class ProjectDetailsModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        [BindProperty(SupportsGet = true)]
        public int Project { get; set; }

        public string? ProjectName { get; set; }

        public List<ProfileMembersData>? profileDataList;
        public bool Exist { get; set; }
        // bool variable to check for the row of the current date leave or not 
        public bool Leave { get; set; }
        public void OnGet()
        {
            int userId = Id;
            int projectNumber = Project;

            List<int> memberIds = new List<int>();
            profileDataList = new List<ProfileMembersData>();

            // store the membersId in the list 
            string connectionString = "Host=localhost;Port=5432;Database=projectpro;Username=postgres;Password=zxcvbnm;";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                string checkExist = "SELECT Count(*) FROM tasks where helper = @id AND date = current_date";
                using (NpgsqlCommand command = new NpgsqlCommand(checkExist, connection))
                {
                    command.Parameters.AddWithValue("@id", userId);
                    int result = Convert.ToInt32(command.ExecuteScalar());
                    Exist = result > 0;
                }
            }

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                string checkExist = "SELECT Count(*) FROM tasks where helper = @id AND date = current_date AND leave = 1 and status = 0";
                using (NpgsqlCommand command = new NpgsqlCommand(checkExist, connection))
                {
                    command.Parameters.AddWithValue("@id", userId);
                    int result = Convert.ToInt32(command.ExecuteScalar());
                    Leave = result > 0;
                }
            }

            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // Query to fetch memberIds where projectid matches Project
                    string selectQuery = "SELECT member FROM members WHERE projectid = @project";

                    using (NpgsqlCommand command = new NpgsqlCommand(selectQuery, connection))
                    {
                        command.Parameters.AddWithValue("@project", projectNumber);

                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Check if the column is not null and contains an array of integers
                                if (!reader.IsDBNull(0) && reader.GetFieldValue<int[]>(0) != null)
                                {
                                    int[] memberIdArray = reader.GetFieldValue<int[]>(0);
                                    memberIds.AddRange(memberIdArray);
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during database operations
                Console.WriteLine("An error occurred: " + ex.Message);
            }


            memberIds.Remove(userId);
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                string projectNameQuery = "SELECT name FROM projects WHERE number = @number";
                using (NpgsqlCommand command = new NpgsqlCommand(projectNameQuery, connection))
                {
                    command.Parameters.AddWithValue("@number", projectNumber);

                    // Execute the query and retrieve the project name
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        ProjectName = result.ToString();
                    }
                    else
                    {
                        // Handle the case where no project with the specified number was found
                        ProjectName = "Project not found"; // You can set a default message here
                    }
                }
            }


            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // Query to fetch profile data where id matches the values in memberIds list
                    string selectQuery = "SELECT id, nickname, image, position FROM profile WHERE id = ANY(@memberIds)";

                    using (NpgsqlCommand command = new NpgsqlCommand(selectQuery, connection))
                    {
                        // Convert the list of memberIds to an array
                        int[] memberIdsArray = memberIds.ToArray();
                        command.Parameters.AddWithValue("@memberIds", memberIdsArray);

                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Create a ProfileData object and populate it with data from the query
                                ProfileMembersData profile = new ProfileMembersData
                                {
                                    Id = reader.GetInt32(0),
                                    MemberNickname = reader.GetString(1),
                                    MemberImage = reader.IsDBNull(2) ? null : reader.GetFieldValue<byte[]>(2),
                                    MemberPosition = reader.GetString(3)
                                };

                                // Add the profile data to the list
                                profileDataList.Add(profile);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during database operations
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        public IActionResult OnPost()
        {
            int userId = Id;
            int projectOwnerId;
            string connectionString = "Host=localhost;Port=5432;Database=projectpro;Username=postgres;Password=zxcvbnm;";
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                string getOwnderId = "select id from projects where number = @number";
                using (NpgsqlCommand command = new NpgsqlCommand(getOwnderId, connection))
                {
                    command.Parameters.AddWithValue("@number", Project);

                    projectOwnerId = (int)command.ExecuteScalar();
                }
                connection.Close();
            }

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                string addLeave = "INSERT INTO tasks (project, owner, helper, leave) VALUES (@project, @owner, @helper, 1)";
                using (NpgsqlCommand command = new NpgsqlCommand(addLeave, connection))
                {
                    command.Parameters.AddWithValue("@project", Project);
                    command.Parameters.AddWithValue("@owner", projectOwnerId);
                    command.Parameters.AddWithValue("@helper", userId);
                    command.ExecuteNonQuery();
                }
            }

            return RedirectToPage("/Home", new { Id = userId });
        }
    }
    
}
