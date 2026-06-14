namespace FixMaster.Bidding.Domain.Entities;

public class Bid
{
    public Guid Id { get; private set; }
    public Guid RequestId { get; private set; }
    public Guid MasterId { get; private set; }
    public decimal Amount { get; private set; }
    public string Description { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public BidStatus Status { get; private set; }

    private Bid() { }

    public Bid(Guid requestId, Guid masterId, decimal amount, string description)
    {
        Id = Guid.NewGuid();
        RequestId = requestId;
        MasterId = masterId;
        Amount = amount;
        Description = description;
        CreatedAt = DateTime.UtcNow;
        Status = BidStatus.Pending;
    }

    public void Accept()
    {
        Status = BidStatus.Accepted;
    }

    public void Reject()
    {
        Status = BidStatus.Rejected;
    }
}

public enum BidStatus
{
    Pending,
    Accepted,
    Rejected
}
