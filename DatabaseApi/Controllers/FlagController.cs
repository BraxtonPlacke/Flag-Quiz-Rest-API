using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using DatabaseApi.Models;
using Microsoft.Extensions.Configuration;

namespace DatabaseApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlagController : ControllerBase
    {
        private readonly string connectionString;

        public FlagController(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet]
        public async Task<IActionResult> GetCountries()
        {
            var countries = new List<FlagModel>();

            await using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                string query = "SELECT Id, CountryName FROM Flags";

                await using (SqlCommand cmd = new SqlCommand(query, conn))
                await using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        countries.Add(new FlagModel
                        {
                            Id = reader.GetString(0),
                            CountryName = reader.GetString(1)
                        });
                    }
                }
            }

            return Ok(countries); // automatically returns JSON
        }

        [HttpGet("random")]
        public async Task<IActionResult> GetRandomCountry()
        {


            await using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                string query = "SELECT TOP 1 Id, CountryName FROM Flags ORDER BY NEWID()";

                await using (SqlCommand cmd = new SqlCommand(query, conn))
                await using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        var country = new FlagModel
                        {
                            Id = reader.GetString(0),
                            CountryName = reader.GetString(1)
                        };
                        return Ok(country);
                    }
                }
            }

            return NotFound();

        }

        [HttpGet("quiz")]
        public async Task<IActionResult> GetQuiz()
        {
            var countries = new List<FlagModel>();
            
            

            await using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                string query = "SELECT TOP 4 Id, CountryName FROM Flags ORDER BY NEWID()";
                await using (SqlCommand cmd = new SqlCommand(query, conn))
                await using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        countries.Add(new FlagModel
                        {
                            Id = reader.GetString(0),
                            CountryName = reader.GetString(1)
                        });
                    }
                }

                if (countries.Count < 4)
                    return StatusCode(500, "Not enough data for quiz");

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

