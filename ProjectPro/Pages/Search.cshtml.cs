using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;
using System;

namespace ProjectPro.Pages
{
    public class SearchModel : PageModel
    {
        [BindProperty]
        public bool applicant { get; set; }

        [BindProperty]
        public bool Check { get; set; }

        [BindProperty]
        public string? Owner { get; set; }

        [BindProperty]
        public int Id { get; set; }

        [BindProperty]
        public string? Name { get; set; }

        [BindProperty]
        public DateTime StartDate { get; set; }

        [BindProperty]
        public DateTime EndDate { get; set; }

        [BindProperty]
        public int Number { get; set; }

        [BindProperty]
        public int Member { get; set; } // Property to store the length of the member array

        public void OnGet()
        {
            if (Request.Query.TryGetValue("projectNumber", out var projectNumberValue)
    && Request.Query.TryGetValue("userId", out var userIdValue))
            {
                if (int.TryParse(projectNumberValue, out var projectNumber)
        && int.TryParse(userIdValue, out var userId))
                {
                    string connectionString = "Host=localhost;Port=5432;Database=projectpro;Username=postgres;Password=zxcvbnm;";
                    // Assuming you have a database connection, replace "YourConnectionString" with your actual connection string
                    using (var connection = new NpgsqlConnection(connectionString))
                    {
                        connection.Open();

                        // Query to retrieve project data based on the hash value
                        string selectQuery = "SELECT p.id, p.name, p.startdate, p.enddate, p.number, pr.nickname " +
                                             "FROM projects AS p " +
                                             "JOIN profile AS pr ON p.id = pr.id " +
                                             "WHERE p.hash = @projectNumber";

                        using (var command = new NpgsqlCommand(selectQuery, connection))
                        {
                            command.Parameters.AddWithValue("@projectNumber", projectNumber);

                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    // Retrieve project data
                                    Id = reader.GetInt32(0);
                                    Name = reader.GetString(1);
                                    StartDate = reader.GetDateTime(2);
                                    EndDate = reader.GetDateTime(3);
                                    Number = reader.GetInt32(4);

                                    // Retrieve Owner if it exists in the database
                                    if (!reader.IsDBNull(5))
                                    {
                                        Owner = reader.GetString(5);
                                    }
                                    else
                                    {
                                        // Handle the case where the Owner is NULL or empty.
                                    }
                                    reader.Close();
                                    // Create a new NpgsqlCommand for the additional query
                                    string additionalQuery = "SELECT (@Id = ANY(m.member)) AS isMember, " +
                                                             "array_length(m.member, 1) AS memberArrayLength " +
                                                             "FROM members AS m " +
                                                             "WHERE m.projectid = @Number";

                                    using (var additionalCommand = new NpgsqlCommand(additionalQuery, connection))
                                    {
                                        additionalCommand.Parameters.AddWithValue("@Number", Number); // Pass the Number as a parameter
                                        additionalCommand.Parameters.AddWithValue("@Id", userId); // Pass the Id as a parameter

                                        using (var additionalReader = additionalCommand.ExecuteReader())
                                        {
                                            if (additionalReader.Read())
                                            {
                                                // Retrieve the isConditionMet value (bool)
                                                Check = additionalReader.GetBoolean(0);

                                                // Retrieve the length of the member array
                                                Member = additionalReader.GetInt32(1);

                                                additionalReader.Close();

                                                // Now you have the isConditionMet value and the length of the member array.
                                            }
                                        }
                                    }
                                    string checkUserQuery = "SELECT EXISTS(SELECT 1 FROM application WHERE userid = @userId)";

                                    using (var checkUserCommand = new NpgsqlCommand(checkUserQuery, connection))
                                    {
                                        checkUserCommand.Parameters.AddWithValue("@userId", userId);
                                        applicant = (bool)checkUserCommand.ExecuteScalar();
                                    }

                                }
                                else
                                {
                                    // Handle the case where no project with the specified hash was found.
                                }
                            }
                        }

                        connection.Close();
                    }
                }
                else
                {
                    // Handle the case where the query parameter is not a valid integer.
                }
            }
            else
            {
                // Handle the case where the projectNumber query parameter is missing.
            }
        }

        public IActionResult OnPost()
        {
            int user = 0;
            if (Request.Query.TryGetValue("projectNumber", out var projectNumberValue)
                && Request.Query.TryGetValue("userId", out var userIdValue))
            {
                if (int.TryParse(projectNumberValue, out var projectNumber)
                    && int.TryParse(userIdValue, out var userId))
                {
                    user = userId;

                    // Define your PostgreSQL connection string
                    string connectionString = "Host=localhost;Port=5432;Database=projectpro;Username=postgres;Password=zxcvbnm;";

                    using (var connection = new NpgsqlConnection(connectionString))
                    {
                        connection.Open();

                        // Retrieve the Number from the projects table based on the projectNumber
                        string selectNumberQuery = "SELECT number FROM projects WHERE hash = @projectNumber";

                        using (var selectNumberCommand = new NpgsqlCommand(selectNumberQuery, connection))
                        {
                            selectNumberCommand.Parameters.AddWithValue("@projectNumber", projectNumber);

                            // Execute the query to retrieve the Number
                            Number = (int)selectNumberCommand.ExecuteScalar();
                        }

                        // Define the SQL query to insert data into the "application" table
                        string insertQuery = "INSERT INTO application (userId, projectNumber, projectHash) VALUES (@userId, @projectNumber, @projectHash)";

                        using (var command = new NpgsqlCommand(insertQuery, connection))
                        {
                            command.Parameters.AddWithValue("@userId", userId);
                            command.Parameters.AddWithValue("@projectNumber", Number); // Use the retrieved Number
                            command.Parameters.AddWithValue("@projectHash", projectNumber);

                            // Execute the INSERT query
                            command.ExecuteNonQuery();
                        }

                        connection.Close();
                    }

                    // Redirect to the Home page or another page after the data is inserted
                    return RedirectToPage("/Home", new { Id = userId });
                }

                // Handle the case where parsing userId and projectNumber failed
                // You can add error handling logic here
            }
            return RedirectToPage("/Home", new { Id = user });
        }


    }
}
