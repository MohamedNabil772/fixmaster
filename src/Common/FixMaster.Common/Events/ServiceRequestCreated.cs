namespace FixMaster.Common.Events;

public record ServiceRequestCreated(
    Guid RequestId,
    string Title,
    string Description,
    string Category,
    decimal Budget,
    Guid ClientId,
    DateTime CreatedAt);
