using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;

namespace ProjectPro.Pages
{
    public class Applicants
    {
        public int userId { get; set; }
        public int projectNumber { get; set; }
    }
    public class ProfileData
    {
        public string? Nickname { get; set; }
        public byte[] Image { get; set; }
        public string? Position { get; set; }
    }

    public class ApplicationsModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public int hashValue { get; set; }

        // list creation
        public List<Applicants>? users { get; set; }
        public List<ProfileData>? profiles { get; set; }

        
        public void OnGet()
        {
            
            string connectionString = "Host=localhost;Port=5432;Database=projectpro;Username=postgres;Password=zxcvbnm;";

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                // Define the SQL query to retrieve the hash value
                string selectQuery = "SELECT hash FROM projects WHERE id = @Id";

                using (var command = new NpgsqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", Id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Retrieve the hash value
                            hashValue = reader.GetInt32(0);
                        }
                        reader.Close();
                        // Handle the case where no rows match the criteria
                    }


                }



                connection.Close();
            }

            users = new List<Applicants>(); // Create a list to store project data
            profiles = new List<ProfileData>();
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                // Define the SQL query to retrieve userids and projectnumbers
                string selectQuery = "SELECT userid, projectnumber FROM application WHERE projecthash = @hashValue";

                using (var command = new NpgsqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@hashValue", hashValue);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Retrieve userids and projectnumbers and process them as needed
                            Applicants user = new Applicants
                            {
                                userId = reader.GetInt32(0),
                                projectNumber = reader.GetInt32(1)
                            };

                            users.Add(user);


                        }
                        // Handle the case where no rows match the criteria
                    }
                }

                connection.Close();
            }

            foreach (var user in users)
            {



                ProfileData profile = GetProfileDataForUserId(user.userId);
                profiles.Add(profile);
            }

        }

        private ProfileData GetProfileDataForUserId(int userId)
        {
            // Define your PostgreSQL connection string
            string connectionString = "Host=localhost;Port=5432;Database=projectpro;Username=postgres;Password=zxcvbnm;";

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                // Define the SQL query to retrieve profile data for the given userId
                string selectQuery = "SELECT nickname, image, position FROM profile WHERE id = @userId";

                using (var command = new NpgsqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Create a ProfileData object and populate it with the retrieved data
                            ProfileData profile = new ProfileData
                            {
                                Nickname = reader.GetString(0),
                                Image = reader["image"] as byte[], // Assuming the image is stored as bytea
                                Position = reader.GetString(2)
                            };
                            return profile;
                        }
                    }
                }

                connection.Close();
            }

            // Return an empty ProfileData object if no data is found
            return new ProfileData();
        }

        public IActionResult OnPost()
        {

            int backId = Id;
            int userId = int.Parse(Request.Form["userId"]);
            int projectNumber = int.Parse(Request.Form["projectNumber"]);
            // Assuming you have a PostgreSQL connection string
            string connectionString = "Host=localhost;Port=5432;Database=projectpro;Username=postgres;Password=zxcvbnm;";


            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                string updateQuery = "UPDATE members SET member = array_append(member, @userId) WHERE projectid = @projectNumber";

                using (NpgsqlCommand command = new NpgsqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@projectNumber", projectNumber);

                    int rowsAffected = command.ExecuteNonQuery();
                    connection.Close();

                    if (rowsAffected > 0)
                    {
                        // Now, use a separate connection for the status update
                        using (var connection1 = new NpgsqlConnection(connectionString))
                        {
                            connection1.Open();

                            string updateStatusQuery = "DELETE FROM application WHERE projectnumber = @projectNumber AND userid = @userId";

                            using (var command1 = new NpgsqlCommand(updateStatusQuery, connection1))
                            {
                                command1.Parameters.AddWithValue("@projectNumber", projectNumber);
                                command1.Parameters.AddWithValue("@userId", userId);

                                int updateRowsAffected = command1.ExecuteNonQuery();

                                if (updateRowsAffected > 0)
                                {
                                    return RedirectToPage("/Home", new { Id = backId });
                                }
                            }
                        }
                    }
                }
            }

            // If none of the updates were successful or there was an issue, return Page()
            return Page();



        }





    }
}
