using Inventory.Api.Domain.DataTransfer;
using Inventory.Api.Domain.Messaging;
using Inventory.Api.Infrastructure.DataTransfer;
using Inventory.Api.Infrastructure.Messaging;
using Microsoft.Extensions.Azure;

namespace Inventory.Api.Infrastructure;

public static class Bootstrapper
{
    public static WebApplicationBuilder RegisterInfrastructure(this WebApplicationBuilder builder, Func<MessageConfig> messageConfigFunc)
    {
        builder.Services.AddSingleton<IJsonService, JsonService>();
        builder.Services.AddSingleton<IMessagePublisher, MessagePublisher>();

        builder.Services.AddAzureClients(factoryBuilder =>
        {
            factoryBuilder.AddQueueServiceClient(messageConfigFunc()?.ConnectionString);
        });
        
        return builder;
    }
}