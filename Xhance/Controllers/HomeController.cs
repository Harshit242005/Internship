using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Xhance.Models;
using MySql.Data.MySqlClient;


namespace Xhance.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Main()
        {

            List<FileModel> files = new List<FileModel>();
            string _connectionString = "server=localhost;Uid=root;pwd=azxcvbnmlkjhgfds;database=Xhance;";
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Files";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            FileModel file = new FileModel
                            {
                                Id = reader.GetInt32("Id"),
                                Name = reader.GetString("Name")
                                
                            };

                            files.Add(file);
                        }
                    }
                }
            }
            return View(files);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}