namespace Inventory.Api.Tests.Models;

public class Student
{
    public int Id { get; }
    public string Name { get; }
    public DateTime DateOfBirth { get; }

    public Student(int id, string name, DateTime dateOfBirth)
    {
        Id = id;
        Name = name;
        DateOfBirth = dateOfBirth;
    }
}