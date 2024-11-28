using Microsoft.EntityFrameworkCore;

namespace EmployeeApplicationApi.Models;

public class Employee
{
    public int Id { get; set; } 
    public string Name { get; set; }
    public string Surname { get; set; }

    public Employee(int id, string name, string surname)
    {
        Id = id;
        Name = name;
        Surname = surname;
    }
}