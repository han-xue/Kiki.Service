using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Kiki.CourierService.Shared.Models;
using MediatR;

namespace Kiki.CourierService.Api.Features.Cost
{
    public class CostQueryHandler
    {
        public class QueryHandler : IRequestHandler<Shared.Features.Cost.Cost.Query, Shared.Features.Cost.Cost.Result>
        {
            private readonly KikicourierserviceContext _context;
            public QueryHandler(KikicourierserviceContext context)
            {
                _context = context;
            }
            public Task<Shared.Features.Cost.Cost.Result> Handle(Shared.Features.Cost.Cost.Query request, CancellationToken cancellationToken)
            {
                var result = new Shared.Features.Cost.Cost.Result(){PackageCostList = new List<Shared.Features.Cost.Cost.PackageCostDetails>()};

                DeliveryTimeEstimation.UpdateEstimatedDeliveryTime(request);
                foreach (var packageDetails in request.PackageList)
                {
                    var offer = _context.Offer.FirstOrDefault(x => x.OfferCode == packageDetails.OfferCode && x.IsActive == true);
                    var costDetails = CostCalculation.GetPackageCostDetails(request.PackageBaseInfo, packageDetails, offer);
                    result.PackageCostList.Add(costDetails);
                }

                return Task.FromResult(result);
            }
        }
    }
}
