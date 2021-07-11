using System;
using Kiki.CourierService.Shared.Models;

// ReSharper disable once IdentifierTypo
namespace Kiki.CourierService.Api.Features.Cost
{
    public static class CostCalculation
    {
        public static double GetOriginalCost(Shared.Features.Cost.Cost.PackageDetails packageDetails, double baseDeliveryCost)
        {
            return baseDeliveryCost + packageDetails.PackageWeight * 10 + packageDetails.PackageDistance * 5;
        }

        public static bool IsOfferValid(Shared.Features.Cost.Cost.PackageDetails packageDetails, Offer offer)
        {
            if (offer == null)
            {
                return false;
            }
            return packageDetails.PackageDistance.IsBetween(offer.DistanceRangeFrom, offer.DistanceRangeTo) &&
                   packageDetails.PackageWeight.IsBetween(offer.WeightRangeFrom, offer.WeightRangeTo);
        }

        public static bool IsBetween(this double valueToCheck, double lowerRange, double upperRange)
        {
            return valueToCheck > lowerRange && valueToCheck < upperRange;
        }

        public static Shared.Features.Cost.Cost.PackageCostDetails GetPackageCostDetails(Shared.Features.Cost.Cost.PackageBaseInfo packageBaseInfo, Shared.Features.Cost.Cost.PackageDetails packageDetails, Offer offer)
        {
            var originalCost = GetOriginalCost(packageDetails, packageBaseInfo.BaseDeliveryCost);
            var costDetails = new Shared.Features.Cost.Cost.PackageCostDetails
            {
                Cost = originalCost,
                Discount = 0,
                PackageName = packageDetails.PackageName,
                EstimatedDeliveryTime = packageDetails.EstimatedTime
            };
            if (offer != null && IsOfferValid(packageDetails, offer))
            {
                UpdateCostDetailsWithOffer(costDetails, originalCost, offer);
            }

            return costDetails;
        }

        private static void UpdateCostDetailsWithOffer(Shared.Features.Cost.Cost.PackageCostDetails costDetails, double originalCost, Offer offer)
        {
            costDetails.Cost = Math.Round(originalCost * (1 - offer.DiscountRate));
            costDetails.Discount = Math.Round(originalCost * offer.DiscountRate);
        }
    }
}
