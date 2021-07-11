using System;
using System.Collections.Generic;
using System.Linq;

namespace Kiki.CourierService.Api.Features.Cost
{
    public class DeliveryTimeEstimation
    {
        public static void UpdateEstimatedDeliveryTime(Shared.Features.Cost.Cost.Query query)
        {
            var listOfShipment = CreateShipment(query);
            ProcessShipment(query, listOfShipment);
        }

        public static List<List<Shared.Features.Cost.Cost.PackageDetails>> CreateShipment(Shared.Features.Cost.Cost.Query query)
        {
            var listOfShipment = new List<List<Shared.Features.Cost.Cost.PackageDetails>>();
            var packagesOrderedByWeight = query.PackageList.OrderBy(x => x.PackageWeight).ToList();
            while (packagesOrderedByWeight.Count > 0)
            {
                if (listOfShipment.Count == 0)
                {
                    CreateNewShipment(listOfShipment, packagesOrderedByWeight);
                }

                else if (listOfShipment.Last().Sum(x => x.PackageWeight) + packagesOrderedByWeight.First().PackageWeight >
                    query.VehicleDetails.MaxCarriableWeight)
                {
                    CreateNewShipment(listOfShipment, packagesOrderedByWeight);
                    //if (listOfShipment.Last().Count == 1)
                    //{
                    //    ReplaceWithHeaviestPackage(packagesOrderedByWeight, listOfShipment, packagesOrderedByWeight.Last());
                    //    CreateNewShipment(listOfShipment, packagesOrderedByWeight);
                    //}
                    //else
                    //{
                    //    var remainingWeight = query.VehicleDetails.MaxCarriableWeight - listOfShipment.Last()
                    //        .Take(listOfShipment.Last().Count - 1).Sum(x => x.PackageWeight);
                    //    var heaviestPackageCarriable = packagesOrderedByWeight
                    //        .Where(x => x.PackageWeight <= remainingWeight).OrderByDescending(x => x.PackageWeight)
                    //        .FirstOrDefault();
                    //    if (heaviestPackageCarriable != null)
                    //    {
                    //        // replace the last package in shipment with the heaviest carriable
                    //        ReplaceWithHeaviestPackage(packagesOrderedByWeight, listOfShipment, heaviestPackageCarriable);
                    //    }

                    //    CreateNewShipment(listOfShipment, packagesOrderedByWeight);
                    //}
                }

                else
                {
                    AddPackageToShipment(listOfShipment, packagesOrderedByWeight);
                }
            }

            return listOfShipment;
        }

        private static void AddPackageToShipment(List<List<Shared.Features.Cost.Cost.PackageDetails>> listOfShipment, List<Shared.Features.Cost.Cost.PackageDetails> packagesOrderedByWeight)
        {
            listOfShipment.Last().Add(packagesOrderedByWeight.First());
            packagesOrderedByWeight.RemoveAt(0);
        }

        private static void CreateNewShipment(List<List<Shared.Features.Cost.Cost.PackageDetails>> listOfShipment, List<Shared.Features.Cost.Cost.PackageDetails> packagesOrderedByWeight)
        {
            listOfShipment.Add(new List<Shared.Features.Cost.Cost.PackageDetails>());

            AddPackageToShipment(listOfShipment, packagesOrderedByWeight);
        }

        private static void ReplaceWithHeaviestPackage(List<Shared.Features.Cost.Cost.PackageDetails> packagesOrderedByWeight, List<List<Shared.Features.Cost.Cost.PackageDetails>> listOfShipment,
            Shared.Features.Cost.Cost.PackageDetails heaviestPackageCarriable)
        {
            packagesOrderedByWeight.Add(listOfShipment.Last().Last());
            var indexOfLastOne = listOfShipment.Last().Count - 1;
            listOfShipment.Last().RemoveAt(indexOfLastOne);
            listOfShipment.Last().Add(heaviestPackageCarriable);
            packagesOrderedByWeight.Remove(heaviestPackageCarriable);
        }

        public static void ProcessShipment(Shared.Features.Cost.Cost.Query query, List<List<Shared.Features.Cost.Cost.PackageDetails>> listOfShipment)
        {
            var listOfVehicleCurrentTime = new double[query.VehicleDetails.NoOfVehicles];
            if (listOfShipment.Count > 0)
            {
                var orderedShipment = listOfShipment.OrderByDescending(x => x.Count).ThenByDescending(x => x.Sum(x => x.PackageWeight));
                foreach (var shipment in orderedShipment)
                {
                    Array.Sort(listOfVehicleCurrentTime);
                    foreach (var package in shipment)
                    {
                        var deliveryTime = Math.Round(package.PackageDistance / query.VehicleDetails.MaxSpeed, 2);
                        query.PackageList.First(x => x.PackageName == package.PackageName).EstimatedTime = Math.Round(deliveryTime + listOfVehicleCurrentTime[0], 2);
                    }

                    listOfVehicleCurrentTime[0] = Math.Round(listOfVehicleCurrentTime[0] + shipment.Max(x => x.EstimatedTime) * 2, 2);
                }
            }
            
            
        }
    }
}
