using EmployeeApplicationApi.Database;
using EmployeeApplicationApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApplicationApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeesController : ControllerBase
    {

        private readonly EmployeeDbContext _dbContext;

        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(ILogger<EmployeesController> logger, EmployeeDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet(Name = "GetEmployees")]
        public async Task<IEnumerable<Employee>> GetEmployees()
        {
           _logger.LogInformation("Requesting all employees");
           
           return await _dbContext.Employees.ToListAsync();
        }
    }
}
