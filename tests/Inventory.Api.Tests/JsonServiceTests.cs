using FluentAssertions;
using Inventory.Api.Domain;
using Inventory.Api.Infrastructure.DataTransfer;
using Inventory.Api.Tests.Models;

namespace Inventory.Api.Tests;

public class JsonServiceTests
{
    [Fact]
    public async Task SerializeValidObject()
    {
        var student = new Student(1, "Cheranga Hatangala", new DateTime(1982, 11, 01));
        var service = new JsonService();
        var operation = await service.SerializeAsync(student).Run();

        operation.IsSucc.Should().BeTrue();
        operation.IfSucc(s => s.Should().NotBeNullOrEmpty());
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