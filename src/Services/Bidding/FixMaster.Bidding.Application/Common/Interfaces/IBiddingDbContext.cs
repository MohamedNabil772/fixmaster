using FixMaster.Bidding.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FixMaster.Bidding.Application.Common.Interfaces;

public interface IBiddingDbContext
{
    DbSet<ServiceRequest> ServiceRequests { get; }
    DbSet<Bid> Bids { get; }
    DbSet<FixMaster.Bidding.Domain.Entities.Feedback> Feedbacks { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
