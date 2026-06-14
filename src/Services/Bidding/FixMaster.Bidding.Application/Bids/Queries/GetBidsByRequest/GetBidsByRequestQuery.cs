using FixMaster.Bidding.Application.Common.Interfaces;
using FixMaster.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FixMaster.Bidding.Application.Bids.Queries.GetBidsByRequest;

public record GetBidsByRequestQuery(Guid RequestId) : IRequest<IEnumerable<BidResponse>>;

public record BidResponse(
    Guid Id,
    Guid RequestId,
    Guid MasterId,
    decimal Amount,
    string Description,
    string Status,
    DateTime CreatedAt);

public class GetBidsByRequestQueryHandler : IRequestHandler<GetBidsByRequestQuery, IEnumerable<BidResponse>>
{
    private readonly IBiddingDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetBidsByRequestQueryHandler(IBiddingDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<BidResponse>> Handle(GetBidsByRequestQuery request, CancellationToken cancellationToken)
    {
        var serviceRequest = await _context.ServiceRequests
            .FirstOrDefaultAsync(r => r.Id == request.RequestId, cancellationToken);

        if (serviceRequest == null)
        {
            throw new Exception("Service request not found.");
        }

        var userId = _currentUserService.UserId;
        var role = _currentUserService.Role;

        IQueryable<Domain.Entities.Bid> bidsQuery = _context.Bids
            .Where(b => b.RequestId == request.RequestId);

        // Apply Privacy Rules
        if (userId == serviceRequest.ClientId.ToString())
        {
            // Owner sees all
        }
        else if (role == "Master")
        {
            // Master sees only their own
            bidsQuery = bidsQuery.Where(b => b.MasterId.ToString() == userId);
        }
        else
        {
            // Others see nothing
            return Enumerable.Empty<BidResponse>();
        }

        return await bidsQuery
            .Select(b => new BidResponse(
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
