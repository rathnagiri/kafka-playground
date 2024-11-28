using EmployeeApplicationApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApplicationApi.Database
{
    public class EmployeeDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }

        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> dbContextOptions) : base(dbContextOptions)
        {
                
        }
    }
}
