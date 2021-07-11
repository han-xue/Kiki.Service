using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Kiki.CourierService.Api.Features.Cost;
using NUnit.Framework;

namespace Kiki.CourierService.Api.Tests.Features.Cost
{
    [TestFixture]
    class DeliveryTimeEstimationTest
    {
        private Shared.Features.Cost.Cost.Query _query;
        [SetUp]
        public void SetupSampleTestData()
        {
            _query = new Shared.Features.Cost.Cost.Query()
            {
                PackageBaseInfo = new Shared.Features.Cost.Cost.PackageBaseInfo()
                {
                    NoOfPackages = 5,
                    BaseDeliveryCost = 100
                },
                PackageList = new List<Shared.Features.Cost.Cost.PackageDetails>()
                {
                    new Shared.Features.Cost.Cost.PackageDetails()
                    {
                        PackageDistance = 30,
                        PackageWeight = 50,
                        OfferCode = "OFR001",
                        PackageName = "PKG1"
                    },
                    new Shared.Features.Cost.Cost.PackageDetails()
                    {
                        PackageDistance = 125,
                        PackageWeight = 75,
                        OfferCode = "OFFR0008",
                        PackageName = "PKG2"
                    },
                    new Shared.Features.Cost.Cost.PackageDetails()
                    {
                        PackageDistance = 100,
                        PackageWeight = 175,
                        OfferCode = "OFFR003",
                        PackageName = "PKG3"
                    },
                    new Shared.Features.Cost.Cost.PackageDetails()
                    {
                        PackageDistance = 60,
                        PackageWeight = 110,
                        OfferCode = "OFFR002",
                        PackageName = "PKG4"
                    },
                    new Shared.Features.Cost.Cost.PackageDetails()
                    {
                        PackageDistance = 95,
                        PackageWeight = 155,
                        OfferCode = "NA",
                        PackageName = "PKG5"
                    },
                },
                VehicleDetails = new Shared.Features.Cost.Cost.VehicleDetails()
                {
                    MaxCarriableWeight = 200,
                    MaxSpeed = 70,
                    NoOfVehicles = 2
                }
            };
        }
        [Test]
        public void CreateShipment_Should_Maximise_No_Of_PackagesIn_Shipment()
        {
            var shipments = DeliveryTimeEstimation.CreateShipment(_query);
            shipments.Count.Should().Be(4);
            shipments.Select(x => x.Count).Max().Should().Be(2);
        }

        [Test]
        public void ProcessShipment_Should_Prefer_Heaviest_Shipment()
        {
            var shipments = DeliveryTimeEstimation.CreateShipment(_query);
            DeliveryTimeEstimation.ProcessShipment(_query, shipments);
            var expectedDeliveryTime = new List<Shared.Features.Cost.Cost.PackageDetails>
            {
                new Shared.Features.Cost.Cost.PackageDetails {EstimatedTime = 0.43, PackageName = "PKG1"},
                new Shared.Features.Cost.Cost.PackageDetails {EstimatedTime = 1.79, PackageName = "PKG2"},
                new Shared.Features.Cost.Cost.PackageDetails {EstimatedTime = 1.43, PackageName = "PKG3"},
                new Shared.Features.Cost.Cost.PackageDetails {EstimatedTime = 4.44, PackageName = "PKG4"},
                new Shared.Features.Cost.Cost.PackageDetails {EstimatedTime = 4.22, PackageName = "PKG5"},
            };
            foreach (var expectedDeliverTime in expectedDeliveryTime)
            {
                _query.PackageList.First(x => x.PackageName == expectedDeliverTime.PackageName).EstimatedTime.Should()
                    .Be(expectedDeliverTime.EstimatedTime);
            }
        }
    }
}
