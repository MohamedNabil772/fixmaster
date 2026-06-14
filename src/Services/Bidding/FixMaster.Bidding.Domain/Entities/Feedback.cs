namespace FixMaster.Bidding.Domain.Entities;

public class Feedback
{
    public Guid Id { get; private set; }
    public Guid RequestId { get; private set; }
    public Guid ClientId { get; private set; }
    public Guid MasterId { get; private set; }
    public int Rating { get; private set; }
    public string Comment { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Feedback() { }

    public Feedback(Guid requestId, Guid clientId, Guid masterId, int rating, string comment)
    {
        if (rating < 1 || rating > 5)
            throw new ArgumentOutOfRangeException(nameof(rating), "Rating must be between 1 and 5.");

        Id = Guid.NewGuid();
        RequestId = requestId;
        ClientId = clientId;
        MasterId = masterId;
        Rating = rating;
        Comment = comment;
        CreatedAt = DateTime.UtcNow;
    }
}
