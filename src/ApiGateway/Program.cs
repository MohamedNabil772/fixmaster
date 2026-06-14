using FixMaster.Common.Logging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
SerilogConfiguration.Configure(builder.Configuration, "ApiGateway");
builder.Host.UseSerilog();

// Add YARP
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

app.UseCors("DefaultPolicy");

// Use Correlation ID Middleware
app.UseMiddleware<CorrelationIdMiddleware>();

app.MapReverseProxy();

app.Run();
