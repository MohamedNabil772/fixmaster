using FixMaster.Bidding.Application.Common.Interfaces;
using FixMaster.Bidding.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FixMaster.Bidding.Infrastructure.Persistence;

public class BiddingDbContext : DbContext, IBiddingDbContext
{
    public BiddingDbContext(DbContextOptions<BiddingDbContext> options) : base(options) { }

    public DbSet<ServiceRequest> ServiceRequests { get; set; }
    public DbSet<Bid> Bids { get; set; }
    public DbSet<Feedback> Feedbacks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BiddingDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
