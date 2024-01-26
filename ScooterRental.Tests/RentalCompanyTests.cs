
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using ScooterRental.Interfaces;

namespace ScooterRental.Tests
{
    public class RentalCompanyTests
    {
        private AutoMocker _mocker;
        private RentalCompany _company;
        private Mock<IScooterService> _scooterServiceMock;
        private Mock<IRentedScooterArchive> _rentedScooterArchiveMock;
        private Mock<IRentalCalculatorService> _rentalCalculatorMock;
        private const string _defaultCompanyName = "tests";

        [SetUp]
        public void Setup()
        {
            _mocker = new AutoMocker();
            _scooterServiceMock = _mocker.GetMock<IScooterService>();
            _rentedScooterArchiveMock = _mocker.GetMock<IRentedScooterArchive>();
            _rentalCalculatorMock = _mocker.GetMock<IRentalCalculatorService>();
            _company = new RentalCompany(_defaultCompanyName, _scooterServiceMock.Object, _rentedScooterArchiveMock.Object, _rentalCalculatorMock.Object);
        }

        [Test]
        public void StartRent_Rent_Existing_Scooter_ScooterIsRented()
        {
            var scooter = new Scooter("1", 0.2m);
            _scooterServiceMock.Setup(s => s.GetScooterById("1")).Returns(scooter) ;

            _company.StartRent("1");

            scooter.IsRented.Should().BeTrue();
        }

        [Test]
        public void EndtRent_StopRenting_Existing_Scooter_ScooterRentStopped()
        {
            var scooter = new Scooter("1", 0.2m) { IsRented = true};
            var now = DateTime.Now;
            var rentalRecord = new RentedScooter(scooter.Id, now.AddMinutes(-20), scooter.PricePerMinute) { RentEnd = now };
            _rentedScooterArchiveMock.Setup(archive => archive.EndRental(scooter.Id, It.IsAny<DateTime>())).Returns(rentalRecord);
            _scooterServiceMock.Setup(s => s.GetScooterById("1")).Returns(scooter);
            _rentalCalculatorMock.Setup(calculator => calculator.CalculateRent(rentalRecord)).Returns(4); ;

            var result = _company.EndRent("1");

            scooter.IsRented.Should().BeFalse();
            result.Should().Be(4);
        }

    }
}
