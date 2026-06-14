using FixMaster.Bidding.Application.Common.Interfaces;
using FixMaster.Common.Events;
using FixMaster.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FixMaster.Bidding.Application.Requests.Commands.SelectMaster;

public record SelectMasterCommand(Guid RequestId, Guid BidId) : IRequest;

public class SelectMasterCommandHandler : IRequestHandler<SelectMasterCommand>
{
    private readonly IBiddingDbContext _context;
    private readonly IPublisher _publisher;
    private readonly ICurrentUserService _currentUserService;

    public SelectMasterCommandHandler(IBiddingDbContext context, IPublisher publisher, ICurrentUserService currentUserService)
    {
        _context = context;
        _publisher = publisher;
        _currentUserService = currentUserService;
    }

    public async Task Handle(SelectMasterCommand request, CancellationToken cancellationToken)
    {
        var serviceRequest = await _context.ServiceRequests
            .FirstOrDefaultAsync(r => r.Id == request.RequestId, cancellationToken);

        if (serviceRequest == null)
        {
            throw new Exception("Service request not found.");
        }

        // Validate ownership
        if (serviceRequest.ClientId != Guid.Parse(_currentUserService.UserId!))
        {
            throw new Exception("Unauthorized to select master for this request.");
        }

        var bids = await _context.Bids
            .Where(b => b.RequestId == request.RequestId)
            .ToListAsync(cancellationToken);

        var selectedBid = bids.FirstOrDefault(b => b.Id == request.BidId);

        if (selectedBid == null)
        {
            throw new Exception("Bid not found.");
        }

        foreach (var bid in bids)
        {
            if (bid.Id == request.BidId)
            {
                bid.Accept();
            }
            else
            {
                bid.Reject();
            }
        }

        serviceRequest.StartService();

        await _context.SaveChangesAsync(cancellationToken);

        await _publisher.Publish(new BidAccepted(
            selectedBid.Id,
            selectedBid.RequestId,
            selectedBid.MasterId,
            selectedBid.Amount), cancellationToken);
    }
}
