namespace FixMaster.Bidding.Domain.Entities;

public class ServiceRequest
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public string Category { get; private set; }
    public decimal Budget { get; private set; }
    public Guid ClientId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public RequestStatus Status { get; private set; }

    private ServiceRequest() { }

    public ServiceRequest(string title, string description, string category, decimal budget, Guid clientId)
    {
        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        Category = category;
        Budget = budget;
        ClientId = clientId;
        CreatedAt = DateTime.UtcNow;
        Status = RequestStatus.Open;
    }

    public void CloseBidding()
    {
        if (Status == RequestStatus.Open)
        {
            Status = RequestStatus.BiddingClosed;
        }
    }

    public void StartService()
    {
        if (Status == RequestStatus.Open || Status == RequestStatus.BiddingClosed)
        {
            Status = RequestStatus.InProgress;
        }
    }
}

public enum RequestStatus
{
    Open,
    BiddingClosed,
    InProgress,
    Completed,
    Cancelled
}
