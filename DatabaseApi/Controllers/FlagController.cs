using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using DatabaseApi.Models;

namespace DatabaseApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlagController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetCountries()
        {
            var countries = new List<FlagModel>();

            using (SqlConnection conn = new SqlConnection(DbConfig.ConnectionString))
            {
                conn.Open();

                string query = "SELECT Id, CountryName FROM Flags";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        countries.Add(new FlagModel
                        {
                            Id = reader["Id"].ToString(),
                            CountryName = reader["CountryName"].ToString()
                        });
                    }
                }
            }

            return Ok(countries); // automatically returns JSON
        }

        [HttpGet("random")]
        public IActionResult GetRandomCountry()
        {


            using (SqlConnection conn = new SqlConnection(DbConfig.ConnectionString))
            {
                conn.Open();
                string query = "SELECT TOP 1 Id, CountryName FROM Flags ORDER BY NEWID()";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var country = new FlagModel
                        {
                            Id = reader["Id"].ToString(),
                            CountryName = reader["CountryName"].ToString()
                        };
                        return Ok(country);
                    }
                }
            }

            return NotFound();

        }

        [HttpGet("quiz")]
        public IActionResult GetQuiz()
        {
            var countries = new List<FlagModel>();

            using (SqlConnection conn = new SqlConnection(DbConfig.ConnectionString))
            {
                conn.Open();
                string query = "SELECT TOP 4 Id, CountryName FROM Flags ORDER BY NEWID()";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        countries.Add(new FlagModel
                        {
                            Id = reader["Id"].ToString(),
                            CountryName = reader["CountryName"].ToString()
                        });
                    }
                }
                var correct = countries[0];
                var random = new Random();
                var shuffled = countries.OrderBy(x => random.Next()).ToList();

                var quiz = new QuizModel
                {
                    Correct = correct,
                    Options = shuffled
                };

                return Ok(quiz);

            }
        }
    }
}

