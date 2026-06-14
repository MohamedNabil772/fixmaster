using FixMaster.Bidding.Application.Common.Interfaces;
using FixMaster.Bidding.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FixMaster.Bidding.Application.Bids.Queries.GetAllBids;

public record GetAllBidsQuery(string? Status = null) : IRequest<List<BidDto>>;

public record BidDto(
    Guid Id,
    Guid RequestId,
    Guid MasterId,
    decimal Amount,
    string Description,
    string Status,
    DateTime CreatedAt);

public class GetAllBidsQueryHandler : IRequestHandler<GetAllBidsQuery, List<BidDto>>
{
    private readonly IBiddingDbContext _context;

    public GetAllBidsQueryHandler(IBiddingDbContext context)
    {
        _context = context;
    }

    public async Task<List<BidDto>> Handle(GetAllBidsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Bids.AsQueryable();

        if (!string.IsNullOrEmpty(request.Status) && Enum.TryParse<BidStatus>(request.Status, true, out var status))
        {
            query = query.Where(b => b.Status == status);
        }

        return await query
            .OrderByDescending(b => b.CreatedAt)
            .Select(b => new BidDto(
                b.Id,
                b.RequestId,
                b.MasterId,
                b.Amount,
                b.Description,
                b.Status.ToString(),
                b.CreatedAt))
            .ToListAsync(cancellationToken);
    }
}
