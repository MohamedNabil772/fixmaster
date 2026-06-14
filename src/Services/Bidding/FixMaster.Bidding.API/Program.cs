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

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<CorrelationIdMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
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
