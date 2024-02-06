
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using ScooterRental.Exceptions;
using ScooterRental.Interfaces;

namespace ScooterRental.Tests
{
    public class RentalCalculatorServiceTests
    {
        private AutoMocker _mocker;
        private Mock<IRentedScooterArchive> _rentedScooterArchiveMock;
        private IRentalCalculatorService _rentalCalculator;
        private readonly Scooter _scooter = new Scooter("1", 0.2m) { IsRented = true };

        private readonly List<RentedScooter> _rentedScooterList = new List<RentedScooter>
        {
            new RentedScooter("1", DateTime.Now.AddMinutes(-90), 0.2m) { RentEnd = DateTime.Now }, //18.0Eur
            new RentedScooter("2", new DateTime(2023, 1, 15, 8, 0, 0), 0.2m) { RentEnd = new DateTime(2023, 1, 15, 8, 30, 0) }, //6Eur
            new RentedScooter("3", DateTime.Now.AddMinutes(-30), 0.2m), //6Eur
            new RentedScooter("4", new DateTime(2024, 3, 10, 14, 0, 0), 0.2m) { RentEnd = new DateTime(2024, 3, 11, 14, 0, 0) },//40Eur
        };

        [SetUp]
        public void setup()
        {
            _mocker = new AutoMocker();
            _rentedScooterArchiveMock = _mocker.GetMock<IRentedScooterArchive>();
            _rentedScooterArchiveMock.Setup(archive => archive.GetAllRentedScooterList()).Returns(_rentedScooterList);
            _rentalCalculator = _mocker.CreateInstance<RentalCalculatorService>();
        }

        [Test]
        public void CalculateRent_Under_Daily_Price_Limit_Should_Calculate_Rent_Correctly()
        {
            //Assert
            DateTime now = DateTime.Now;
            var rentalRecord = new RentedScooter(_scooter.Id, now.AddMinutes(-99), _scooter.PricePerMinute) { RentEnd = now };

            //Act
            decimal rent = _rentalCalculator.CalculateRent(rentalRecord);

            //Assert
            rent.Should().Be(19.8m);
        }

        [Test]
        public void CalculateRent_Over_Daily_Price_Limit_Should_Calculate_Rent_Correctly()
        {
            //Assert
            DateTime startRentTime = new DateTime(2024, 01, 26, 08, 00, 00);
            DateTime endRantTime = startRentTime.AddMinutes(121);
            var rentalRecord = new RentedScooter(_scooter.Id, startRentTime, _scooter.PricePerMinute) { RentEnd = endRantTime };

            //Act
            decimal rent = _rentalCalculator.CalculateRent(rentalRecord);

            //Assert
            rent.Should().Be(20m);
        }

        [Test]
        public void CalculateRent_Rent_Over_Midnight_Should_Calculate_Rent_Corrcetly()
        {
            //Assert
            DateTime startRentTime = new DateTime(2024, 01, 26, 23, 10, 00);
            DateTime endRantTime = startRentTime.AddMinutes(121);
            var rentalRecord = new RentedScooter(_scooter.Id, startRentTime, _scooter.PricePerMinute) { RentEnd = endRantTime };

            //Act
            decimal rent = _rentalCalculator.CalculateRent(rentalRecord);

            //Assert
            rent.Should().Be(24.2m);
        }

        [Test]
        public void CalculateRent_Rent_Over_Multiple_Days_Should_Calculate_Rent_Correctly()
        {
            //Assert
            DateTime startRentTime = new DateTime(2024, 01, 26, 23, 10, 00);
            DateTime endRantTime = new DateTime(2024, 01, 30, 00, 10, 00);
            var rentalRecord = new RentedScooter(_scooter.Id, startRentTime, _scooter.PricePerMinute) { RentEnd = endRantTime };

            //Act
            decimal rent = _rentalCalculator.CalculateRent(rentalRecord);

            //Assert
            rent.Should().Be(72m);
        }

        [Test]
        public void CalculateRent_RentalRecord_Without_EndDate_Should_Calculate_Rent_Correctly()
        {
            //Assert
            DateTime startRentTime = DateTime.Now.AddMinutes(-19);
            var rentalRecord = new RentedScooter(_scooter.Id, startRentTime, _scooter.PricePerMinute);

            //Act
            decimal rent = _rentalCalculator.CalculateRent(rentalRecord);

            //Assert
            rent.Should().Be(3.8m);
        }

        [TestCase(null, true, 70)]
        [TestCase(2023, true, 6)]
        [TestCase(2024, false, 58)]
        [TestCase(2024, true, 64)]
        [TestCase(null, false, 64)]
        public void CalculateIncome_Should_Calculate_Income_Correctly(int? year, bool includeNotCompletedRentals, decimal expectedIncome)
        {
            //Act
            decimal income = _rentalCalculator.CalculateIncome(year, includeNotCompletedRentals);

            //Assert
            income.Should().Be(expectedIncome);
        }

        [Test]
        public void CalculateIncome_Invalid_ProvidedYear_Should_Throw_InvalidYearException()
        {
            //Act
            Action action = () => _rentalCalculator.CalculateIncome(DateTime.Now.AddYears(1).Year, true);

            //Assert
            action.Should().Throw<InvalidYearException>();
        }
    }
}
