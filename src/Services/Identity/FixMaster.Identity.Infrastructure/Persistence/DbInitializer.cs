using FixMaster.Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FixMaster.Identity.Infrastructure.Persistence;

public static class DbInitializer
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<FixMasterIdentityDbContext>();
        await context.Database.EnsureCreatedAsync();

        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

        string[] roles = { "SuperAdmin", "Admin", "Client", "Master" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // Seed SuperAdmin
        await SeedUserAsync(userManager, "superadmin@fixmaster.com", "Super", "Admin", "SuperAdmin", "SuperAdmin");
        // Seed Admin
        await SeedUserAsync(userManager, "admin@fixmaster.com", "Admin", "Fix", "Master", "Admin");
        // Seed Client
        await SeedUserAsync(userManager, "client@fixmaster.com", "Client", "John", "Doe", "Client");
        // Seed Master
        await SeedUserAsync(userManager, "master@fixmaster.com", "Master", "Bob", "Builder", "Master");
    }

    private static async Task SeedUserAsync(UserManager<User> userManager, string email, string firstName, string lastName, string password, string role)
    {
        if (await userManager.FindByEmailAsync(email) == null)
        {
            var user = new User
            {
                UserName = email,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, "Password123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, role);
            }
        }
    }
}
