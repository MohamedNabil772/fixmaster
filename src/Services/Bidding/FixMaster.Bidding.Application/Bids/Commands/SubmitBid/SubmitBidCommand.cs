using FixMaster.Bidding.Application.Common.Interfaces;
using FixMaster.Bidding.Domain.Entities;
using FixMaster.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FixMaster.Bidding.Application.Bids.Commands.SubmitBid;

public record SubmitBidCommand(
    Guid RequestId,
    decimal Amount,
    string Description) : IRequest<Guid>;

public class SubmitBidCommandHandler : IRequestHandler<SubmitBidCommand, Guid>
{
    private readonly IBiddingDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public SubmitBidCommandHandler(IBiddingDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Guid> Handle(SubmitBidCommand request, CancellationToken cancellationToken)
    {
        var serviceRequest = await _context.ServiceRequests
            .FirstOrDefaultAsync(r => r.Id == request.RequestId, cancellationToken);

        if (serviceRequest == null)
        {
            throw new Exception("Service request not found.");
        }

        if (serviceRequest.Status != RequestStatus.Open)
        {
            throw new Exception("Bidding is closed for this request.");
        }

        var masterId = Guid.Parse(_currentUserService.UserId!);
        
        var bid = new Bid(
            request.RequestId,
            masterId,
            request.Amount,
            request.Description);

        _context.Bids.Add(bid);
        await _context.SaveChangesAsync(cancellationToken);

        return bid.Id;
    }
}
