using FixMaster.Bidding.Application.Bids.Queries.GetAllBids;
using FixMaster.Bidding.Application.Common.Interfaces;
using FixMaster.Bidding.Application.Common.Models;
using FixMaster.Bidding.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FixMaster.Bidding.Application.Bids.Queries.GetBidsPaginated;

public record GetBidsPaginatedQuery(
    int PageNumber = 1,
    int PageSize = 10,
    string? Status = null) : IRequest<PaginatedList<BidDto>>;

public class GetBidsPaginatedQueryHandler : IRequestHandler<GetBidsPaginatedQuery, PaginatedList<BidDto>>
{
    private readonly IBiddingDbContext _context;

    public GetBidsPaginatedQueryHandler(IBiddingDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<BidDto>> Handle(GetBidsPaginatedQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Bids.AsQueryable();

        if (!string.IsNullOrEmpty(request.Status) && Enum.TryParse<BidStatus>(request.Status, true, out var status))
        {
            query = query.Where(b => b.Status == status);
        }

        var count = await query.CountAsync(cancellationToken);
        
        var items = await query
            .OrderByDescending(b => b.CreatedAt)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(b => new BidDto(
                b.Id,
                b.RequestId,
                b.MasterId,
                b.Amount,
                b.Description,
                b.Status.ToString(),
                b.CreatedAt))
            .ToListAsync(cancellationToken);

        return new PaginatedList<BidDto>(items, count, request.PageNumber, request.PageSize);
    }
}
