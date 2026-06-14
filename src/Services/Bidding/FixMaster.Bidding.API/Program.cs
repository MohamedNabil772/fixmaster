using FixMaster.Bidding.Application;
using FixMaster.Bidding.Infrastructure;
using FixMaster.Common.Logging;
using FixMaster.Common.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
SerilogConfiguration.Configure(builder.Configuration, "BiddingService");
builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:5266")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

// Initialize Database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<FixMaster.Bidding.Infrastructure.Persistence.BiddingDbContext>();
        await context.Database.EnsureCreatedAsync();
    }
    catch (Exception ex)
    {
        Log.Error(ex, "An error occurred while initializing the Bidding database");
    }
}

app.UseCors("DefaultPolicy");

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<CorrelationIdMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

try
{
    Log.Information("Starting Bidding Service");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Bidding Service terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
