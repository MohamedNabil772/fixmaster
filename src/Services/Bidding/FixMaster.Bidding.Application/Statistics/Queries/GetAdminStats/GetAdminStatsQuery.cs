using FixMaster.Bidding.Application.Common.Interfaces;
using FixMaster.Bidding.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FixMaster.Bidding.Application.Statistics.Queries.GetAdminStats;

public record GetAdminStatsQuery(
    string? FilterType = "year", // day, month, year
    string? Service = null) : IRequest<AdminStatsDto>;

public record AdminStatsDto(
    int TotalUsers,
    int TotalBids,
    int TotalMasters,
    decimal TotalEarnings,
    List<ChartDataPoint> BidsByService,
    List<ChartDataPoint> RequestsByService,
    List<ChartDataPoint> MastersByService,
    List<ChartDataPoint> TimelineData);

public record ChartDataPoint(string Label, decimal Value);

public class GetAdminStatsQueryHandler : IRequestHandler<GetAdminStatsQuery, AdminStatsDto>
{
    private readonly IBiddingDbContext _context;

    public GetAdminStatsQueryHandler(IBiddingDbContext context)
    {
        _context = context;
    }

    public async Task<AdminStatsDto> Handle(GetAdminStatsQuery request, CancellationToken cancellationToken)
    {
        // For a true microservices app, "TotalUsers" and "TotalMasters" would come from the Identity service.
        // For this implementation, we'll estimate or use available data in Bidding (e.g., unique ClientIds/MasterIds).
        
        var totalBids = await _context.Bids.CountAsync(cancellationToken);
        var totalMasters = await _context.Bids.Select(b => b.MasterId).Distinct().CountAsync(cancellationToken);
        var totalUsers = await _context.ServiceRequests.Select(r => r.ClientId).Distinct().CountAsync(cancellationToken);
        
        var totalEarnings = await _context.Bids
            .Where(b => b.Status == BidStatus.Accepted)
            .SumAsync(b => b.Amount, cancellationToken);

        // Bids by Service (Category)
        var bidsByService = await _context.Bids
            .Join(_context.ServiceRequests, b => b.RequestId, r => r.Id, (b, r) => new { b, r })
            .GroupBy(x => x.r.Category)
            .Select(g => new ChartDataPoint(g.Key, g.Count()))
            .ToListAsync(cancellationToken);

        // Requests by Service
        var requestsByService = await _context.ServiceRequests
            .GroupBy(r => r.Category)
            .Select(g => new ChartDataPoint(g.Key, g.Count()))
            .ToListAsync(cancellationToken);

        // Masters by Service (approximated by where they bid)
        var mastersByService = await _context.Bids
            .Join(_context.ServiceRequests, b => b.RequestId, r => r.Id, (b, r) => new { b, r })
            .GroupBy(x => x.r.Category)
            .Select(g => new ChartDataPoint(g.Key, g.Select(x => x.b.MasterId).Distinct().Count()))
            .ToListAsync(cancellationToken);

        // Timeline Data (Simplified example)
        var timelineData = await _context.Bids
            .GroupBy(b => b.CreatedAt.Date)
            .Select(g => new ChartDataPoint(g.Key.ToString("yyyy-MM-dd"), g.Count()))
            .Take(10)
            .ToListAsync(cancellationToken);

        return new AdminStatsDto(
            totalUsers,
            totalBids,
            totalMasters,
            totalEarnings,
            bidsByService,
            requestsByService,
            mastersByService,
            timelineData);
    }
}
