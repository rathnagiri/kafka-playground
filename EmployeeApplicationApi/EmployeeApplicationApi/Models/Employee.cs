using Microsoft.EntityFrameworkCore;

namespace EmployeeApplicationApi.Models;

public class Employee(Guid id, string name, string surname)
{
    public Guid Id { get; set; } = id;
    public string Name { get; set; } = name;
    public string Surname { get; set; } = surname;
}