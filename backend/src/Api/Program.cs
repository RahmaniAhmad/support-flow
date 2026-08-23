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

builder.Services.AddExceptionHandling();

builder.Services.AddOpenApi();

builder.Services.AddDbContext<SupportFlowDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("Database"),
        npgsqlOptions =>
        {
            npgsqlOptions.UseVector();
        });
});

builder.Services.AddApi();
builder.Services.AddInfrastructure(builder.Configuration);

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
    options.AddPolicy("AllowNextJsApp", policy =>
    {
        policy
            .WithOrigins(allowedOrigins)
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

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowNextJsApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapApplicationEndpoints();

app.Run();