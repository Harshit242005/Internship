using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;

namespace ProjectPro.Pages
{
    public class MembersProfileData
    {
        public int Id { get; set; }
        public string? MemberNickname { get; set; }
        public byte[]? MemberImage { get; set; }
        public string? MemberPosition { get; set; }
    }
    public class MembersModel : PageModel
    {
        [BindProperty(SupportsGet = true)] // This attribute binds query parameters
        public int Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Project { get; set; }

        // list to store the profile table data for each id 
        public List<MembersProfileData>? profileDataList;
        // getting project name
        public string? ProjectName { get; set; }
        public void OnGet()
        {
            List<int> memberIds = new List<int>();
            profileDataList = new List<MembersProfileData>();
            string connectionString = "Host=localhost;Port=5432;Database=projectpro;Username=postgres;Password=zxcvbnm;";

            try
            {
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
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during database operations
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            memberIds.Remove(Id);
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


            // using this id list we would be getting the image, name , position kind of data from the profile so we would create a class which will hold these datas
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
                                MembersProfileData profile = new MembersProfileData
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

            // Now, the profileDataList contains profile data for the members with matching ids
        }
    }
}
