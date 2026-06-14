using FixMaster.Bidding.Domain.Entities;
using FixMaster.Common.Specifications;

namespace FixMaster.Bidding.Domain.Specifications;

public class ServiceRequestByCategorySpecification : Specification<ServiceRequest>
{
    public ServiceRequestByCategorySpecification(string category) 
        : base(x => x.Category == category)
    {
        ApplyOrderByDescending(x => x.CreatedAt);
    }
}
