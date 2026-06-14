namespace FixMaster.Common.Events;

public record BidAccepted(
    Guid BidId,
    Guid RequestId,
    Guid MasterId,
    decimal Amount);
