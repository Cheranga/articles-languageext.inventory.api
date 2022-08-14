using Inventory.Api.Domain.Messaging;
using Inventory.Api.Features.AddInventory;
using Inventory.Api.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
RegisterDependencies(builder);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

void RegisterDependencies(WebApplicationBuilder applicationBuilder)
{
    applicationBuilder.Services.AddSingleton<IAddInventoryService, AddInventoryService>();
    
    var messageConfig = applicationBuilder.Configuration.GetSection(nameof(MessageConfig)).Get<MessageConfig>();
    applicationBuilder.Services.AddSingleton(messageConfig);

    applicationBuilder.RegisterInfrastructure(() => messageConfig);
}