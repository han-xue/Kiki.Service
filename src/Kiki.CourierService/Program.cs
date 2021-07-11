using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kiki.CourierService.Shared.Features.Cost;
using Refit;
using Microsoft.Extensions.Configuration;

namespace Kiki.CourierService
{
    class Program
    {
        private static readonly Cost.Query CostQuery = new Cost.Query()
        {
            PackageBaseInfo = new Cost.PackageBaseInfo(),
            PackageList = new List<Cost.PackageDetails>(),
            VehicleDetails = new Cost.VehicleDetails()
        };
        static async Task Main(string[] args)
        {
            var cfg = BuildConfig();
            var offerService = RestService.For<ICostService>( cfg.KikiServiceApi.Host);
            
            ReadAndParseInfo();
            var cost = await offerService.Cost(CostQuery);
            Console.WriteLine("Estimated package cost and delivery time:");
            foreach (var packageCostDetails in cost.PackageCostList)
            {
                Console.WriteLine("{0} {1} {2} {3}", packageCostDetails.PackageName, packageCostDetails.Discount, packageCostDetails.Cost, Math.Round(packageCostDetails.EstimatedDeliveryTime, 2) );
            }
        }

        private static KikiServiceConfig BuildConfig()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env}.json", true, true)
                .AddEnvironmentVariables();

            var config = builder.Build();
            var cfg = config.Get<KikiServiceConfig>();
            return cfg;
        }

        private static void ReadAndParseInfo()
        {
            while (true)
            {
                Console.WriteLine("Please enter base cost and number of packages in the format \"base_delivery_cost no_of_packges\". eg. 100 3");
                var baseInfo = Console.ReadLine();
                if (baseInfo != null && InfoParser.InfoParser.ParsePackageBaseInfo(baseInfo, CostQuery))
                {
                    ReadAndParsePackagesInfo();
                }
                else
                {
                    continue;
                }

                break;
            }
        }

        private static void ReadAndParsePackagesInfo()
        {
            Console.WriteLine("Please enter package details in the format \"pkg_id1 pkg_weight1_in_kg distance1_in_km offer_code1\". Press Enter twice to finish entering package details");
            string packageInfo;
            var numberOfPackageDetailsEntered = 0;
            while ( !string.IsNullOrEmpty(packageInfo = Console.ReadLine())
                    //&& numberOfPackageDetailsEntered < CostQuery.PackageBaseInfo.NoOfPackages
                    )
            {
                if (!InfoParser.InfoParser.ParsePackageInfo(packageInfo, CostQuery))
                {
                    Console.WriteLine(
                        "Details was not entered correctly.Please enter this package details in the format \"pkg_id1 pkg_weight1_in_kg distance1_in_km offer_code1\". Press Enter twice to finish entering package details");
                }

                numberOfPackageDetailsEntered++;
            }
            ReadAndParseVehicleInfo();
        }

        private static void ReadAndParseVehicleInfo()
        {
            while (true)
            {
                Console.WriteLine("Please enter vehicle details in the format \"no_of_vehicles max_speed max_carriable_weight\". eg. 2 70 200");
                var baseInfo = Console.ReadLine();
                if (baseInfo != null && InfoParser.InfoParser.ParseVehicleInfo(baseInfo, CostQuery))
                {
                    break;
                }
            }
        }
    }
}
