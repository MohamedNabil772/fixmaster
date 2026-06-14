using FixMaster.Bidding.Application.Common.Interfaces;
using FixMaster.Bidding.Domain.Entities;
using FixMaster.Bidding.Domain.Specifications;
using FixMaster.Common.Specifications;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FixMaster.Bidding.Application.Requests.Queries.GetRequests;

public record GetRequestsQuery(string? Category = null) : IRequest<List<ServiceRequestDto>>;

public record ServiceRequestDto(
    Guid Id,
    string Title,
    string Description,
    string Category,
    decimal Budget,
    string Status);

public class GetRequestsQueryHandler : IRequestHandler<GetRequestsQuery, List<ServiceRequestDto>>
{
    private readonly IBiddingDbContext _context;

    public GetRequestsQueryHandler(IBiddingDbContext context)
    {
        _context = context;
    }

    public async Task<List<ServiceRequestDto>> Handle(GetRequestsQuery request, CancellationToken cancellationToken)
    {
        IQueryable<ServiceRequest> query = _context.ServiceRequests;

        if (!string.IsNullOrEmpty(request.Category))
        {
            var spec = new ServiceRequestByCategorySpecification(request.Category);
            query = SpecificationEvaluator<ServiceRequest>.GetQuery(query, spec);
        }

        return await query
            .Select(r => new ServiceRequestDto(
                r.Id,
                r.Title,
                r.Description,
                r.Category,
                r.Budget,
                r.Status.ToString()))
            .ToListAsync(cancellationToken);
    }
}
