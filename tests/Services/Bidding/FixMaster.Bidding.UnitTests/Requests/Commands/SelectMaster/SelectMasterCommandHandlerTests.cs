using FixMaster.Bidding.Application.Requests.Commands.SelectMaster;
using FixMaster.Bidding.Domain.Entities;
using FixMaster.Bidding.Infrastructure.Persistence;
using FixMaster.Common.Interfaces;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Xunit;

namespace FixMaster.Bidding.UnitTests.Requests.Commands.SelectMaster;

public class SelectMasterCommandHandlerTests
{
    private readonly BiddingDbContext _context;
    private readonly IPublisher _publisher;
    private readonly ICurrentUserService _currentUserService;
    private readonly SelectMasterCommandHandler _handler;

    public SelectMasterCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<BiddingDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new BiddingDbContext(options);
        _publisher = Substitute.For<IPublisher>();
        _currentUserService = Substitute.For<ICurrentUserService>();
        _handler = new SelectMasterCommandHandler(_context, _publisher, _currentUserService);
    }

    [Fact]
    public async Task Handle_ShouldAcceptBid_WhenUserIsOwner()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var requestId = Guid.NewGuid();
        var bidId = Guid.NewGuid();

        var serviceRequest = new ServiceRequest("Test Title", "Test Desc", "Plumbing", 100, clientId);
        // We need to set the Id and other properties if they are private set.
        // Actually, the constructor sets them.
        
        // Use reflection or just use the object if possible. 
        // ServiceRequest constructor: public ServiceRequest(string title, string description, string category, decimal budget, Guid clientId)
        // It sets Id = Guid.NewGuid(). So we need to use that Id.
        
        var request = new ServiceRequest("Title", "Desc", "Cat", 100, clientId);
        _context.ServiceRequests.Add(request);

        var bid = new Bid(request.Id, Guid.NewGuid(), 90, "I can do it");
        // Similar for Bid
        _context.Bids.Add(bid);
        await _context.SaveChangesAsync(default);

        _currentUserService.UserId.Returns(clientId.ToString());

        var command = new SelectMasterCommand(request.Id, bid.Id);

        // Act
        await _handler.Handle(command, default);

        // Assert
        var updatedBid = await _context.Bids.FindAsync(bid.Id);
        updatedBid!.Status.Should().Be(BidStatus.Accepted);
        
        var otherBids = await _context.Bids.Where(b => b.Id != bid.Id).ToListAsync();
        otherBids.Should().AllSatisfy(b => b.Status.Should().Be(BidStatus.Rejected));
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenUserIsNotOwner()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var otherUserId = Guid.NewGuid();
        
        var request = new ServiceRequest("Title", "Desc", "Cat", 100, clientId);
        _context.ServiceRequests.Add(request);

        var bid = new Bid(request.Id, Guid.NewGuid(), 90, "I can do it");
        _context.Bids.Add(bid);
        await _context.SaveChangesAsync(default);

        _currentUserService.UserId.Returns(otherUserId.ToString());

        var command = new SelectMasterCommand(request.Id, bid.Id);

        // Act
        var act = () => _handler.Handle(command, default);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Unauthorized to select master for this request.");
    }
}
