using Serilog;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

namespace FixMaster.Common.Logging
{
    public static class SerilogConfiguration
    {
        public static void Configure(IConfiguration configuration, string serviceName)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Service", serviceName)
                .WriteTo.Console()
                .WriteTo.Seq(configuration["Seq:ServerUrl"] ?? "http://localhost:5341")
                .CreateLogger();
        }
    }
}
