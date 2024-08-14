using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace EmployeeService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public EmployeesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            string query = "EXEC GetEmployees";
            string resultJson = string.Empty;
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    using (SqlDataReader reader = await myCommand.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            resultJson += reader.GetString(0);
                        }
                    }
                }
            }

            return new ContentResult
            {
                Content = resultJson,
                ContentType = "application/json"
            };
        }
    }
}
