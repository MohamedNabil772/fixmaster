using FixMaster.Bidding.Application.Bids.Commands.SubmitBid;
using FixMaster.Bidding.Domain.Entities;
using FixMaster.Bidding.Infrastructure.Persistence;
using FixMaster.Common.Interfaces;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Xunit;

namespace FixMaster.Bidding.UnitTests.Bids.Commands.SubmitBid;

public class SubmitBidCommandHandlerTests
{
    private readonly BiddingDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly SubmitBidCommandHandler _handler;

    public SubmitBidCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<BiddingDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new BiddingDbContext(options);
        _currentUserService = Substitute.For<ICurrentUserService>();
        _handler = new SubmitBidCommandHandler(_context, _currentUserService);
    }

    [Fact]
    public async Task Handle_ShouldCreateBid_WhenRequestIsOpen()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var masterId = Guid.NewGuid();
        
        var request = new ServiceRequest("Title", "Desc", "Cat", 100, clientId);
        _context.ServiceRequests.Add(request);
        await _context.SaveChangesAsync(default);

        _currentUserService.UserId.Returns(masterId.ToString());

        var command = new SubmitBidCommand(request.Id, 90, "I can do it");

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.Should().NotBeEmpty();
        
        var bid = await _context.Bids.FindAsync(result);
        bid.Should().NotBeNull();
        bid!.Amount.Should().Be(90);
        bid.MasterId.Should().Be(masterId);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenRequestIsClosed()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var masterId = Guid.NewGuid();
        
        var request = new ServiceRequest("Title", "Desc", "Cat", 100, clientId);
        request.CloseBidding();
        _context.ServiceRequests.Add(request);
        await _context.SaveChangesAsync(default);

        _currentUserService.UserId.Returns(masterId.ToString());

        var command = new SubmitBidCommand(request.Id, 90, "I can do it");

        // Act
        var act = () => _handler.Handle(command, default);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Bidding is closed for this request.");
    }
}
