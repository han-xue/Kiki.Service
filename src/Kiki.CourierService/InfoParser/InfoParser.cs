using System.Linq;
using Kiki.CourierService.Shared.Features.Cost;

namespace Kiki.CourierService.InfoParser
{
    public static class InfoParser
    {
        public static bool ParsePackageBaseInfo(string packageBaseInfo, Cost.Query costQuery)
        {
            var info = packageBaseInfo.Split();
            if (info.Length == 2 && double.TryParse(info.ElementAtOrDefault(0), out var baseDeliveryCost) && int.TryParse(info.ElementAtOrDefault(1), out var noOfPackages))
            {
                costQuery.PackageBaseInfo.BaseDeliveryCost = baseDeliveryCost;
                costQuery.PackageBaseInfo.NoOfPackages = noOfPackages;
                return true;
            }

            return false;
        }

        public static bool ParsePackageInfo(string packageInfo, Cost.Query costQuery)
        {
            var info = packageInfo.Split();
            if (info.Length == 4 && double.TryParse(info.ElementAtOrDefault(1), out var packageWeigh) && int.TryParse(info.ElementAtOrDefault(2), out var distance))
            {
                var packageDetails = new Cost.PackageDetails()
                {
                    PackageName = info.ElementAtOrDefault(0),
                    PackageWeight = packageWeigh,
                    PackageDistance = distance,
                    OfferCode = info.ElementAtOrDefault(3)
                };
                costQuery.PackageList.Add(packageDetails);
                return true;
            }

            return false;
        }

        public static bool ParseVehicleInfo(string vehicleInfo, Cost.Query costQuery)
        {

            var info = vehicleInfo.Split();
            if (info.Length == 3 && int.TryParse(info.ElementAtOrDefault(0), out var noOfVehicle) && double.TryParse(info.ElementAtOrDefault(1), out var maxSpeed) && double.TryParse(info.ElementAtOrDefault(2), out var maxCarriable))
            {
                costQuery.VehicleDetails.NoOfVehicles = noOfVehicle;
                costQuery.VehicleDetails.MaxSpeed= maxSpeed;
                costQuery.VehicleDetails.MaxCarriableWeight= maxCarriable;
                return true;
            }

            return false;
        }
    }
}
