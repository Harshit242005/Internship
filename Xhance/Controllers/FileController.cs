using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Xhance.Models;

namespace Xhance.Controllers
{
    public class FileController : Controller
    {
        private readonly string _connectionString;

        public FileController()
        {
            _connectionString = "server=localhost;Uid=root;pwd=azxcvbnmlkjhgfds;database=Xhance;";
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(FileModel file)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO Files (Name, Stuff) VALUES (@name, @content)";
                    command.Parameters.AddWithValue("@name", file.Name);
                    command.Parameters.AddWithValue("@content", file.Content);

                    command.ExecuteNonQuery();
                }
            }

            return RedirectToAction("Main", "Home");
        }

        [HttpGet]
        public IActionResult Search(string searchTerm)
        {
            List<FileModel> searchResults = new List<FileModel>();

            // Perform the search operation based on the searchTerm
            // Populate the searchResults list with the matching files

            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchResults = SearchFilesInDatabase(searchTerm);
            }

            return Json(searchResults);
        }

        private List<FileModel> SearchFilesInDatabase(string searchTerm)
        {
            List<FileModel> searchResults = new List<FileModel>();

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Files WHERE Name LIKE @searchTerm";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@searchTerm", searchTerm + "%");

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            FileModel file = new FileModel
                            {
                                Id = reader.GetInt32("Id"),
                                Name = reader.GetString("Name"),
                                Content = reader.IsDBNull(reader.GetOrdinal("Stuff")) ? null : reader.GetString("Stuff")
                            };

                            searchResults.Add(file);
                        }
                    }
                }
            }

            return searchResults;
        }


        // this is the code to save the text of the file in the file of the table 
        [HttpPost]
        public IActionResult SaveText(string fileName, string textContent)
        {
            // Retrieve the file from the database based on the file name
            FileModel file = GetFileByName(fileName);

            if (file != null)
            {
                // Update the file's content
                file.Content = textContent;

                // Save the changes to the database
                UpdateFile(file);

                return Json(new { success = true });
            }

            return Json(new { success = false, message = "File not found." });
        }

        private FileModel GetFileByName(string fileName)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Files WHERE Name = @fileName";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@fileName", fileName);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            FileModel file = new FileModel
                            {
                                Id = reader.GetInt32("Id"),
                                Name = reader.GetString("Name"),
                                Content = reader.IsDBNull(reader.GetOrdinal("Stuff")) ? null : reader.GetString("Stuff")
                            };

                            return file;
                        }
                    }
                }
            }

            return null;
        }

        private void UpdateFile(FileModel file)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string query = "UPDATE Files SET Stuff = @content WHERE Id = @id";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@content", file.Content);
                    command.Parameters.AddWithValue("@id", file.Id);

                    command.ExecuteNonQuery();
                }
            }
        }

        [HttpGet]
        public IActionResult GetFileContent(string fileName)
        {
            string content = string.Empty;

            // Retrieve the file content from the database based on the fileName
            // You can modify this code to fetch the content from your database table

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT Stuff FROM Files WHERE Name = @fileName";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@fileName", fileName);

                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        content = result.ToString();
                    }
                }
            }

            return Content(content); // Return the content as a response
        }

        // here comes the code for deleting the file 
        [HttpPost]
        public IActionResult Delete(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return BadRequest(new { message = "File name is empty." });
            }

            try
            {
                // Delete the file from the database based on the fileName
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();

                    string deleteQuery = "DELETE FROM Files WHERE Name = @fileName";
                    using (MySqlCommand deleteCommand = new MySqlCommand(deleteQuery, connection))
                    {
                        deleteCommand.Parameters.AddWithValue("@fileName", fileName);
                        deleteCommand.ExecuteNonQuery();
                    }
                }

                return Ok(new { message = "File deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the file.", error = ex.Message });
            }
        }

    }


}

