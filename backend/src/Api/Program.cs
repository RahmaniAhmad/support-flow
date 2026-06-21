using Api.DependencyInjection;
using Api.Extensions;
using Infrastructure.DependencyInjection;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// OpenAPI / Swagger
builder.Services.AddOpenApi();

// Database
builder.Services.AddDbContext<SupportFlowDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("Database"));
});

builder.Services.AddApi();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});


var app = builder.Build();

// OpenAPI
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Authentication / Authorization
app.UseAuthentication();
app.UseAuthorization();

// Endpoints
app.MapEndpoints();
app.MapTicketEndpoints();
app.MapKnowledgeBaseEndpoints();

app.Run();