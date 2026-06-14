using FixMaster.Bidding.Application.Common.Interfaces;
using FixMaster.Bidding.Domain.Entities;
using FixMaster.Common.Events;
using FixMaster.Common.Interfaces;
using MediatR;

namespace FixMaster.Bidding.Application.Requests.Commands.CreateRequest;

public record CreateRequestCommand(
    string Title,
    string Description,
    string Category,
    decimal Budget) : IRequest<Guid>;

public class CreateRequestCommandHandler : IRequestHandler<CreateRequestCommand, Guid>
{
    private readonly IBiddingDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPublisher _publisher;

    public CreateRequestCommandHandler(IBiddingDbContext context, ICurrentUserService currentUserService, IPublisher publisher)
    {
        _context = context;
        _currentUserService = currentUserService;
        _publisher = publisher;
    }

    public async Task<Guid> Handle(CreateRequestCommand request, CancellationToken cancellationToken)
    {
        var clientId = Guid.Parse(_currentUserService.UserId!);

        var serviceRequest = new ServiceRequest(
            request.Title,
            request.Description,
            request.Category,
            request.Budget,
            clientId);

        _context.ServiceRequests.Add(serviceRequest);
        await _context.SaveChangesAsync(cancellationToken);

        await _publisher.Publish(new ServiceRequestCreated(
            serviceRequest.Id,
            serviceRequest.Title,
            serviceRequest.Description,
            serviceRequest.Category,
            serviceRequest.Budget,
            serviceRequest.ClientId,
            serviceRequest.CreatedAt), cancellationToken);
        
        return serviceRequest.Id;
    }
}
