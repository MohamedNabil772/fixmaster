using FixMaster.Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FixMaster.Identity.Infrastructure.Persistence;

public class FixMasterIdentityDbContext : IdentityDbContext<User>
{
    public FixMasterIdentityDbContext(DbContextOptions<FixMasterIdentityDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<User>(entity =>
        {
            entity.Property(e => e.FirstName).HasMaxLength(50).IsRequired();
            entity.Property(e => e.LastName).HasMaxLength(50).IsRequired();
        });
    }
}
