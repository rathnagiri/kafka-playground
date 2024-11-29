using System.Text.Json;
using Confluent.Kafka;
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

        [HttpPost]
        public async Task<ActionResult<Employee>> CreateEmployee(string name, string surname)
        {
            var employee = new Employee(Guid.NewGuid(), name, surname);
            _dbContext.Employees.Add(employee);
            await _dbContext.SaveChangesAsync();
            var serialize = JsonSerializer.Serialize(employee);
            _logger.LogInformation($"saving employee {serialize}");

            // message to kafka
            
            var message = new Message<string, string>()
            {
                Key = employee.Id.ToString(),
                Value = serialize
            };

            // client
            var producerConfig = new ProducerConfig()
            {
                BootstrapServers = "kafka:9092",
                Acks = Acks.All
            };

            // producer
            using (var producer = new ProducerBuilder<string, string>(producerConfig).Build())
            {
                try
                {
                    var deliveryResult = await producer.ProduceAsync("employeeTopic", message);
                    _logger.LogInformation($"Delivered message to Topic: {deliveryResult.Topic} | Partition : {deliveryResult.TopicPartition} | value {deliveryResult.Value}");
                }
                catch (ProduceException<string, string> e)
                {
                    _logger.LogError($"Delivery Failed: {e.Error.Reason}");
                }
            }

            return CreatedAtAction(nameof(CreateEmployee), new { id = employee.Id }, employee);
        }
    }
}
