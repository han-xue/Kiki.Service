using System.Collections.Generic;
using MediatR;

namespace Kiki.CourierService.Shared.Features.Cost
{
    public class Cost
    {
        public class PackageBaseInfo
        {
            public double BaseDeliveryCost { get; set; }
            public int NoOfPackages { get; set; }
        }

        public class Query : IRequest<Result>
        {
            //[Query(CollectionFormat.Multi)]
            public List<PackageDetails> PackageList { get; set; }

            public PackageBaseInfo PackageBaseInfo { get; set; }

            public VehicleDetails VehicleDetails { get; set; }
        }

        public class PackageDetails
        {
            public double PackageWeight { get; set; }
            public double PackageDistance { get; set; }
            public string OfferCode { get; set; }
            public string PackageName { get; set; }
            public double EstimatedTime { get; set; }
        }

        public class VehicleDetails
        {
            public int NoOfVehicles { get; set; }
            public double MaxSpeed { get; set; }
            public double MaxCarriableWeight { get; set; }
        }

        public class Result
        {
            public List<PackageCostDetails> PackageCostList { get; set; }
        };

        public class PackageCostDetails
        {
            public string PackageName { get; set; }
            public double Discount { get; set; }
            public double Cost { get; set; }
            public double EstimatedDeliveryTime { get; set; }
        }
    }
}