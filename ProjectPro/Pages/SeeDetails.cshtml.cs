using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;

namespace ProjectPro.Pages
{
    // class to hold the data for the members of the project
    public class ProjectMembersProfileData
    {
        public int Id { get; set; }
        public string? MemberNickname { get; set; }
        public byte[]? MemberImage { get; set; }
        public string? MemberPosition { get; set; }
    }
    public class SeeDetailsModel : PageModel
    {

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Project { get; set; }

        // initialize the array class for the 
        public List<ProjectMembersProfileData>? profileDataList;
        // to hold the name of the project
        public string? ProjectName { get; set; }
        public void OnGet()
        {

            // this to hold the data that will will be stored in the project
            List<int> memberIds = new List<int>();
            profileDataList = new List<ProjectMembersProfileData>();
            string connectionString = "Host=localhost;Port=5432;Database=projectpro;Username=postgres;Password=zxcvbnm;";

            // code to get the id of the members of the project
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                // Query to fetch memberIds where projectid matches Project
                string selectQuery = "SELECT member FROM members WHERE projectid = @project";

                using (NpgsqlCommand command = new NpgsqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@project", Project);

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
            // remove the id of the owner so he can see the employees in the project only
            memberIds.Remove(Id);
            // to get the project name from the database
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                string projectNameQuery = "SELECT name FROM projects WHERE number = @number";
                using (NpgsqlCommand command = new NpgsqlCommand(projectNameQuery, connection))
                {
                    command.Parameters.AddWithValue("@number", Project);

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

            // Getting the data of the members in the profile list
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
                            ProjectMembersProfileData profile = new ProjectMembersProfileData
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

        public IActionResult OnPost()
        {
            int userId = Id;
            if (int.TryParse(Request.Form["Id"], out int PersonId))
            {
                string connectionString = "Host=localhost;Port=5432;Database=projectpro;Username=postgres;Password=zxcvbnm;";
                // run query to delete the member id from the group 
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    string deleteMember = "UPDATE members SET member = ARRAY_REMOVE(member, @userId) WHERE projectid = @projectId";
                    using (NpgsqlCommand command = new NpgsqlCommand(deleteMember, connection))
                    {
                        command.Parameters.AddWithValue("@userId", PersonId);
                        command.Parameters.AddWithValue("@projectId", Project);
                        command.ExecuteNonQuery();

                    }
                }
                return RedirectToPage("/Home", new { Id = userId });
            }
            else
            {
                return Page(); // Return to the same page or redirect as needed
            }
        }
    }
}
