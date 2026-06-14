using FixMaster.Bidding.Application.Common.Interfaces;
using FixMaster.Bidding.Infrastructure.Persistence;
using FixMaster.Common.Interfaces;
using FixMaster.Common.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FixMaster.Bidding.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BiddingDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IBiddingDbContext>(provider => provider.GetRequiredService<BiddingDbContext>());

        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}
