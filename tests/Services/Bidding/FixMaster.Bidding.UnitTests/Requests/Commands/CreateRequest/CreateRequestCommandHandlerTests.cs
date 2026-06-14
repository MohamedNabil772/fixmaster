using FixMaster.Bidding.Application.Requests.Commands.CreateRequest;
using FixMaster.Bidding.Infrastructure.Persistence;
using FixMaster.Common.Events;
using FixMaster.Common.Interfaces;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Xunit;

namespace FixMaster.Bidding.UnitTests.Requests.Commands.CreateRequest;

public class CreateRequestCommandHandlerTests
{
    private readonly BiddingDbContext _context;
    private readonly IPublisher _publisher;
    private readonly ICurrentUserService _currentUserService;
    private readonly CreateRequestCommandHandler _handler;

    public CreateRequestCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<BiddingDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new BiddingDbContext(options);
        _publisher = Substitute.For<IPublisher>();
        _currentUserService = Substitute.For<ICurrentUserService>();
        _handler = new CreateRequestCommandHandler(_context, _currentUserService, _publisher);
    }

    [Fact]
    public async Task Handle_ShouldCreateRequest_AndPublishEvent()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        _currentUserService.UserId.Returns(clientId.ToString());

        var command = new CreateRequestCommand(
            "Fix my sink",
            "It's leaking everywhere",
            "Plumbing",
            150);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.Should().NotBeEmpty();
        
        var request = await _context.ServiceRequests.FindAsync(result);
        request.Should().NotBeNull();
        request!.Title.Should().Be(command.Title);
        request.ClientId.Should().Be(clientId);

        await _publisher.Received(1).Publish(
            Arg.Is<ServiceRequestCreated>(e => 
                e.RequestId == result && 
                e.ClientId == clientId && 
                e.Title == command.Title),
            default);
    }
}
