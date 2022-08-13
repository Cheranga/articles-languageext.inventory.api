using FluentAssertions;
using Inventory.Domain;
using Inventory.Domain.Messaging;
using Inventory.Infrastructure.DataTransfer;

namespace Inventory.Infrastructure.Tests;

public record Student(int Id, string Name, DateTime DateOfBirth) : IMessage;

public class JsonServiceTests
{
    [Fact]
    public async Task SerializeValidObject()
    {
        var student = new Student(1, "Cheranga Hatangala", new DateTime(1982, 11, 01));
        var service = new JsonService();
        var serializedContent = (await service.SerializeAsync(student).Run()).IfFail(error => error.Message);

        serializedContent.Should().NotBeNull().And.NotBeEmpty();
    }

    [Fact]
    public async Task SerializeNullObject()
    {
        Student student = null;
        var service = new JsonService();
        var operation = await service.SerializeAsync(student).Run();
        operation.IsFail.Should().BeTrue();
        operation.IfFail(error => error.Code.Should().Be(ErrorCodes.InvalidData));
    }

    [Fact]
    public async Task DeserializeValidObject()
    {
        var student = new Student(1, "Cheranga Hatangala", new DateTime(1982, 11, 01));
        var service = new JsonService();
        var serializedContent = (await service.SerializeAsync(student).Run()).IfFail(error => error.Message);

        var expectedStudent = (await service.DeserializeAsync<Student>(serializedContent).Run()).IfFail(_ => new Student(666, "error", DateTime.Now));

        expectedStudent.Should().BeEquivalentTo(student);
    }
}