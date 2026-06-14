using FixMaster.Bidding.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FixMaster.Bidding.Application.Feedback.Queries.GetProviderRating;

public record GetProviderRatingQuery(Guid MasterId) : IRequest<ProviderRatingDto>;

public record ProviderRatingDto(decimal AverageRating, int TotalFeedbacks);

public class GetProviderRatingQueryHandler : IRequestHandler<GetProviderRatingQuery, ProviderRatingDto>
{
    private readonly IBiddingDbContext _context;

    public GetProviderRatingQueryHandler(IBiddingDbContext context)
    {
        _context = context;
    }

    public async Task<ProviderRatingDto> Handle(GetProviderRatingQuery request, CancellationToken cancellationToken)
    {
        var feedbacks = await _context.Feedbacks
            .Where(f => f.MasterId == request.MasterId)
            .ToListAsync(cancellationToken);

        if (!feedbacks.Any())
            return new ProviderRatingDto(0, 0);

        var average = (decimal)feedbacks.Average(f => f.Rating);
        return new ProviderRatingDto(average, feedbacks.Count);
    }
}
