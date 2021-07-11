using FluentAssertions;
using Kiki.CourierService.Api.Features.Cost;
using Kiki.CourierService.Shared.Models;
using NUnit.Framework;

namespace Kiki.CourierService.Api.Tests.Features.Cost
{
    [TestFixture]
    class CostCalculationTest
    {
        private Shared.Features.Cost.Cost.PackageDetails _packageDetails;
        private Offer _offer001;
        private Offer _offer003;
        private Shared.Features.Cost.Cost.PackageBaseInfo _packageBaseInfo;
        [SetUp]
        public void SetupSampleData()
        {
            _packageDetails = new Shared.Features.Cost.Cost.PackageDetails()
            {
                PackageDistance = 5,
                PackageWeight = 5,
                OfferCode = "OFR001",
                PackageName = "PKG1"
            };
            _offer001 = new Offer()
            {
                DistanceRangeFrom = 70,
                DistanceRangeTo = 200,
                WeightRangeFrom = 0,
                WeightRangeTo = 200,
                OfferCode = "OFR002",
                DiscountRate = 10
            };
            _offer003 = new Offer()
            {
                DistanceRangeFrom = 10,
                DistanceRangeTo = 150,
                WeightRangeFrom = 50,
                WeightRangeTo = 250,
                OfferCode = "OFR003",
                DiscountRate = 5
            };
            _packageBaseInfo = new Shared.Features.Cost.Cost.PackageBaseInfo()
                {
                    NoOfPackages = 3,
                    BaseDeliveryCost = 100
                };
        }

        [Test]
        public void IsOfferValid_False_If_Offer_Is_Null()
        {
            Assert.IsFalse(CostCalculation.IsOfferValid(_packageDetails, null));
        }

        [TestCase(201, 75, false)]
        [TestCase(199, 75, true)]
        [TestCase(200, 75, false)]
        [TestCase(199, 69, false)]
        [TestCase(199, 70, false)]
        public void IsOfferValid_Returns_Correct_Value(double packageWeight, double packageDistance, bool expectedValue)
        {
            _packageDetails.PackageDistance = packageDistance;
            _packageDetails.PackageWeight = packageWeight;
            CostCalculation.IsOfferValid(_packageDetails, _offer001).Should().Be(expectedValue);
        }

        [TestCase(10,10,20,false)]
        [TestCase(11,10,20,true)]
        [TestCase(20,10,20,false)]
        [TestCase(9,10,20,false)]
        [TestCase(21,10,20,false)]
        public void IsBetween_True_if_value_is_bigger_than_lower_range(double valueToCheck, double lowerRange, double upperRange, bool expectedValue)
        {
            Assert.AreEqual(expectedValue, valueToCheck.IsBetween(lowerRange, upperRange));
        }

        [Test]
        public void GetPackageCostDetails_Generated_Correctly_with_invalid_Offer()
        {
            var costDetails = CostCalculation.GetPackageCostDetails(_packageBaseInfo, _packageDetails, _offer001);
            var expectedCostDetails = new Shared.Features.Cost.Cost.PackageCostDetails()
            {
                Discount = 0,
                Cost = 175,
                PackageName = _packageDetails.PackageName
            };

            costDetails.Should().Equals(expectedCostDetails);
        }

        [Test]
        public void GetPackageCostDetails_Generated_Correctly_with_valid_Offer()
        {
            _packageDetails.PackageDistance = 100;
            _packageDetails.PackageWeight = 10;
            var costDetails = CostCalculation.GetPackageCostDetails(_packageBaseInfo, _packageDetails, _offer003);
            var expectedCostDetails = new Shared.Features.Cost.Cost.PackageCostDetails()
            {
                Discount = 35,
                Cost = 665,
                PackageName = _packageDetails.PackageName
            };

            costDetails.Should().Equals(expectedCostDetails);
        }
    }
}
