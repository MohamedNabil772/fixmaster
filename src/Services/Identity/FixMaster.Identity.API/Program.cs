using FixMaster.Common.Logging;
using FixMaster.Identity.Application;
using FixMaster.Identity.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
SerilogConfiguration.Configure(builder.Configuration, "IdentityService");
builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Seed Roles
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await FixMaster.Identity.Infrastructure.Persistence.DbInitializer.SeedRolesAsync(services);
    }
    catch (Exception ex)
    {
        Log.Error(ex, "An error occurred while seeding roles");
    }
}

// Configure the HTTP request pipeline.
app.UseMiddleware<CorrelationIdMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

try
{
    Log.Information("Starting Identity Service");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Identity Service terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
