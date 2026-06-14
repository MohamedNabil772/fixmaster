using FixMaster.Bidding.Application.Common.Interfaces;
using FixMaster.Bidding.Domain.Entities;
using FixMaster.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FixMaster.Bidding.Application.Feedback.Commands.SubmitFeedback;

public record SubmitFeedbackCommand(
    Guid RequestId,
    int Rating,
    string Comment) : IRequest<Guid>;

public class SubmitFeedbackCommandHandler : IRequestHandler<SubmitFeedbackCommand, Guid>
{
    private readonly IBiddingDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public SubmitFeedbackCommandHandler(IBiddingDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Guid> Handle(SubmitFeedbackCommand request, CancellationToken cancellationToken)
    {
        var serviceRequest = await _context.ServiceRequests
            .FirstOrDefaultAsync(r => r.Id == request.RequestId, cancellationToken);

        if (serviceRequest == null)
            throw new Exception("Service request not found.");

        if (serviceRequest.ClientId.ToString() != _currentUserService.UserId)
            throw new Exception("Only the client who created the request can provide feedback.");

        // In a real scenario, check if status is 'Completed'
        // if (serviceRequest.Status != RequestStatus.Completed)
        //    throw new Exception("Feedback can only be provided for completed services.");

        var acceptedBid = await _context.Bids
            .FirstOrDefaultAsync(b => b.RequestId == request.RequestId && b.Status == BidStatus.Accepted, cancellationToken);

        if (acceptedBid == null)
            throw new Exception("No accepted bid found for this request.");

        var feedback = new Domain.Entities.Feedback(
            request.RequestId,
            serviceRequest.ClientId,
            acceptedBid.MasterId,
            request.Rating,
            request.Comment);

        _context.Feedbacks.Add(feedback);
        await _context.SaveChangesAsync(cancellationToken);

        return feedback.Id;
    }
}
