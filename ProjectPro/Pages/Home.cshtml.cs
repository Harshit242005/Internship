using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;

namespace ProjectPro.Pages
{
    public class Project
    {
        public int Number { get; set; }
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime StartDate { get; set; }
        // Add more properties as needed
        public DateTime EndDate { get; set; }
        public int Hash { get; set; }
    }

    public class HomeModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        public bool checkExist { get; set; }
        public int projectNumber { get; set; }

        public bool idExistsInProfile;
        public string? Base64Image { get; set; }
        public List<Project>? userProjects { get; set; }
        public List<int> MemberIds { get; set; } = new List<int>();
        public List<byte[]> MemberImages { get; set; } = new List<byte[]>();

        // data of the project with the projectNumber
        public string? projectName { get; set; }
        public DateTime projectStartDate { get; set; }
        public DateTime projectEndDate { get; set; }
        public int projectHashNumber { get; set; }
        public int projectOwnerId { get; set; }
        public byte[]? projectOwner { get; set; }

        public bool keepCheck { get; set; }
        public void OnGet()
        {
            int userId = Id;

            // Create a connection to the database
            string connectionString = "Host=localhost;Port=5432;Database=projectpro;Username=postgres;Password=zxcvbnm;";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                string checkQuery = "SELECT COUNT(*) FROM projects WHERE id = @userId";
                using (NpgsqlCommand command = new NpgsqlCommand(checkQuery, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);

                    // ExecuteScalar returns the count of rows where id matches the userId
                    int rowCount = Convert.ToInt32(command.ExecuteScalar());

                    // If rowCount is greater than 0, it means there's at least one matching row
                    checkExist = rowCount > 0;
                }
            }

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                // Query to check if the userId exists in the members table and retrieve projectNumber
                string checkQuery = "SELECT projectid FROM members WHERE @userId = ANY(member)";
                using (NpgsqlCommand command = new NpgsqlCommand(checkQuery, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);

                    // ExecuteScalar to retrieve the projectNumber
                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        // Now result is of type int
                        projectNumber = (int)result;
                        keepCheck = true;
                    }
                    else
                    {
                        // Handle the case where result is null (no project found)
                        projectNumber = 0;
                        keepCheck = false;
                    }
                }


            }

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                // Query to retrieve project data from the projects table
                string projectQuery = "SELECT name, startdate, enddate, hash, id FROM projects WHERE number = @projectNumber";
                using (NpgsqlCommand projectCommand = new NpgsqlCommand(projectQuery, connection))
                {
                    projectCommand.Parameters.AddWithValue("@projectNumber", projectNumber);

                    using (NpgsqlDataReader projectReader = projectCommand.ExecuteReader())
                    {
                        if (projectReader.Read())
                        {
                            projectName = projectReader.GetString(0);
                            projectStartDate = projectReader.GetDateTime(1);
                            projectEndDate = projectReader.GetDateTime(2);
                            projectHashNumber = projectReader.GetInt32(3);
                            projectOwnerId = projectReader.GetInt32(4);
                        }
                    }
                }

                // Query to retrieve image data from the profile table where id = projectOwner
                string imageQuery = "SELECT image FROM profile WHERE id = @projectOwner";
                using (NpgsqlCommand imageCommand = new NpgsqlCommand(imageQuery, connection))
                {
                    imageCommand.Parameters.AddWithValue("@projectOwner", projectOwnerId);

                    using (NpgsqlDataReader imageReader = imageCommand.ExecuteReader())
                    {
                        if (imageReader.Read())
                        {
                            projectOwner = imageReader["image"] as byte[];
                        }
                    }
                }
            }

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                // Query to check if the Id exists in the profile table
                string checkIdQuery = "SELECT COUNT(*) FROM profile WHERE id = @userId";

                using (NpgsqlCommand command = new NpgsqlCommand(checkIdQuery, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);

                    // Execute the query to check if the Id exists
                    long count = (long)command.ExecuteScalar(); // Change int to long

                    if (count > 0)
                    {
                        // Id exists in the profile table, you can set a flag or variable
                        idExistsInProfile = true;

                        // Now you can use idExistsInProfile in your conditional logic
                    }
                    else
                    {
                        // Id does not exist in the profile table
                        idExistsInProfile = false;

                        // Now you can use idExistsInProfile in your conditional logic
                    }
                }

                connection.Close();
            }

            /* to get the image data we woukd call another function */
            byte[] imageData;

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                // Query to fetch the image data based on the user's ID
                string selectQuery = "SELECT image FROM profile WHERE id = @id";

                using (NpgsqlCommand command = new NpgsqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@id", userId);
                    imageData = (byte[])command.ExecuteScalar();
                }

                connection.Close();
            }

            if (imageData != null)
            {
                // Convert the image data to a Base64 string
                string base64Image = Convert.ToBase64String(imageData);
                Base64Image = base64Image;
            }
            else
            {
                Base64Image = null;
            }

            //// we would create a variable that can hold and handle the variable to check if the user have created and not ended a project or not
            //using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            //{
            //    connection.Open();
            //    string checkQuery = "SELECT COUNT(*) FROM projects WHERE status = 0 AND id = @id";
            //    using (NpgsqlCommand command = new NpgsqlCommand(checkQuery, connection))
            //    {
            //        command.Parameters.AddWithValue("@id", userId); // Use double quotes for parameter names

            //        // Use ExecuteScalar to retrieve a single value (COUNT(*))
            //        int count = Convert.ToInt32(command.ExecuteScalar());

            //        if (count > 0)
            //        {
            //            keepCheck = false;
            //        }
            //        else
            //        {
            //            keepCheck = true;
            //        }
            //    }
            //}



            /* here we would write the code to get the projects data and sent it the front end */

            userProjects = new List<Project>(); // Create a list to store project data

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                // Query to fetch project data based on the user's ID
                string selectQuery = "SELECT number, id, name, startdate, enddate, hash FROM projects WHERE id = @id";

                using (NpgsqlCommand command = new NpgsqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@id", userId);

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Project project = new Project
                            {
                                Number = reader.GetInt32(0),
                                Id = reader.GetInt32(1),
                                Name = reader.GetString(2),
                                StartDate = reader.GetDateTime(3),
                                EndDate = reader.GetDateTime(4),
                                Hash = reader.GetInt32(5),
                            };

                            userProjects.Add(project);
                        }
                    }
                }

                connection.Close();
            }

            // Now that we have all project IDs, fetch member IDs and images
            // Now that we have all project IDs, fetch member IDs and images
            foreach (var project in userProjects)
            {
                int projectId = project.Number;

                // Clear the MemberIds list for the current project
                MemberIds.Clear();

                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // Query to fetch member IDs associated with the project
                    string selectMembersQuery = "SELECT member FROM members WHERE projectid = @projectId";

                    using (NpgsqlCommand memberCommand = new NpgsqlCommand(selectMembersQuery, connection))
                    {
                        memberCommand.Parameters.AddWithValue("@projectId", projectId);

                        using (NpgsqlDataReader memberReader = memberCommand.ExecuteReader())
                        {
                            while (memberReader.Read())
                            {
                                var memberArray = memberReader.GetFieldValue<int[]>(0);
                                MemberIds.AddRange(memberArray);
                            }
                        }
                    }

                    // Fetch member images for the current project's members
                    foreach (int memberId in MemberIds)
                    {
                        string selectImageQuery = "SELECT image FROM profile WHERE id = @memberId";

                        using (NpgsqlCommand imageCommand = new NpgsqlCommand(selectImageQuery, connection))
                        {
                            imageCommand.Parameters.AddWithValue("@memberId", memberId);

                            using (NpgsqlDataReader imageReader = imageCommand.ExecuteReader())
                            {
                                if (imageReader.Read())
                                {
                                    byte[] imageBytes = imageReader.GetFieldValue<byte[]>(0);
                                    MemberImages.Add(imageBytes);
                                }
                            }
                        }
                    }

                    connection.Close();
                }
            }






        }

        public IActionResult OnGetSearch(int projectNumber, int userId)
        {
            // Here, you can calculate the hash value based on the project number
            // You can replace this with your actual hash calculation logic
            int hashValue = projectNumber;
            int userCheckId = userId;
            // Redirect to the Search page with the hash and id values as query parameters
            return RedirectToPage("/Search", new { projectNumber = hashValue, userId = userCheckId });
        }




    }
}
