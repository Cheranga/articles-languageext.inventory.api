using Azure;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using FluentAssertions;
using Inventory.Api.Domain;
using Inventory.Api.Domain.Messaging;
using Inventory.Api.Infrastructure.DataTransfer;
using Inventory.Api.Infrastructure.Messaging;
using Inventory.Api.Tests.Models;
using Moq;

namespace Inventory.Api.Tests;

public class MessagePublisherTests
{
    [Fact]
    public async Task ErrorWhenGettingQueueClientThroughServiceClient()
    {
        var messageConfig = new MessageConfig
        {
            QueueName = "inventory"
        };

        var mockedQueueServiceClient = new Mock<QueueServiceClient>();
        mockedQueueServiceClient.Setup(x => x.GetQueueClient(It.IsAny<string>())).Throws(new Exception("cannot get queue client"));


        var messagePublisher = new MessagePublisher(new JsonService(), mockedQueueServiceClient.Object);
        var operation = await messagePublisher.PublishAsync("queue", new PublishableMessage<Student>(new Student(1, "Cheranga Hatangala", new DateTime(1982, 11, 1)))).Run();

        operation.IsFail.Should().BeTrue();
        operation.IfFail(error => error.Code.Should().Be(ErrorCodes.ErrorWhenGettingQueueClient));
    }

    [Fact]
    public async Task ErrorOccursWhenPublishingMessage()
    {
        var messageConfig = new MessageConfig
        {
            QueueName = "inventory"
        };

        var mockedQueueClient = new Mock<QueueClient>();
        mockedQueueClient.Setup(x => x.CreateIfNotExistsAsync(It.IsAny<IDictionary<string, string>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Mock.Of<Response>);
        mockedQueueClient.Setup(x => x.SendMessageAsync(It.IsAny<string>()))
            .Throws(new Exception("cannot publish message"));

        var mockedResponse = new Mock<Response<QueueClient>>();
        mockedResponse.Setup(x => x.Value).Returns(mockedQueueClient.Object);

        var mockedQueueServiceClient = new Mock<QueueServiceClient>();
        mockedQueueServiceClient.Setup(x => x.GetQueueClient(It.IsAny<string>())).Returns(mockedQueueClient.Object);


        var messagePublisher = new MessagePublisher(new JsonService(), mockedQueueServiceClient.Object);
        var operation = await messagePublisher.PublishAsync("queue", new PublishableMessage<Student>(new Student(1, "Cheranga Hatangala", new DateTime(1982, 11, 1)))).Run();

        operation.IsSucc.Should().BeFalse();
        operation.IfFail(error => error.Code.Should().Be(ErrorCodes.ErrorWhenPublishingMessage));
    }

    [Fact]
    public async Task PublishMessage()
    {
        var mockedQueueClient = new Mock<QueueClient>();
        mockedQueueClient.Setup(x => x.CreateIfNotExistsAsync(It.IsAny<IDictionary<string, string>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Mock.Of<Response>);
        mockedQueueClient.Setup(x => x.SendMessageAsync(It.IsAny<string>()))
            .ReturnsAsync(Mock.Of<Response<SendReceipt>>);

        var mockedResponse = new Mock<Response<QueueClient>>();
        mockedResponse.Setup(x => x.Value).Returns(mockedQueueClient.Object);

        var mockedQueueServiceClient = new Mock<QueueServiceClient>();
        mockedQueueServiceClient.Setup(x => x.GetQueueClient(It.IsAny<string>())).Returns(mockedQueueClient.Object);


        var messagePublisher = new MessagePublisher(new JsonService(), mockedQueueServiceClient.Object);
        var operation = await messagePublisher.PublishAsync("queue", new PublishableMessage<Student>(new Student(1, "Cheranga Hatangala", new DateTime(1982, 11, 1)))).Run();

        operation.IsSucc.Should().BeTrue();
    }
}