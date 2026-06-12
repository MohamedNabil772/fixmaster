using FixMaster.Common.Logging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
SerilogConfiguration.Configure(builder.Configuration, "ApiGateway");
builder.Host.UseSerilog();

// Add YARP
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

// Use Correlation ID Middleware
app.UseMiddleware<CorrelationIdMiddleware>();

app.MapReverseProxy();

app.Run();
