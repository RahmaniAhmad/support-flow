using Api.DependencyInjection;
using Api.Extensions;
using Infrastructure.DependencyInjection;
using Infrastructure.Extensions;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<Api.Options.CorsOptions>(
    builder.Configuration.GetSection(Api.Options.CorsOptions.SectionName));

var allowedOrigins = builder.Configuration
    .GetSection(Api.Options.CorsOptions.SectionName + ":AllowedOrigins")
    .Get<string[]>() ?? [];

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

builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN";

    if (builder.Environment.IsDevelopment())
    {
        options.Cookie.Name = "Antiforgery";
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    }
    else
    {
        options.Cookie.Name = "__Host-Antiforgery";
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    }

    options.Cookie.HttpOnly = true;
    options.Cookie.Path = "/";
    options.Cookie.SameSite = SameSiteMode.Lax;
});

builder.Services.AddCors(options =>
       {
           options.AddPolicy("AllowNextJsApp",
               builder =>
               {
                   builder.WithOrigins(allowedOrigins)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
               });
       });

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(
        new System.Text.Json.Serialization.JsonStringEnumConverter()
    );
});

var app = builder.Build();

await app.InitializeDatabaseAsync();

// OpenAPI
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("AllowNextJsApp");

app.UseRouting();

// Authentication / Authorization
app.UseAuthentication();
app.UseAuthorization();

// Endpoints
app.MapApplicationEndpoints();

app.Run();