
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using ScooterRental.Exceptions;
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
        public void Property_Name_Should_Return_Correct_Name()
        {
            //Arrange
            var name = _company.Name;

            //Assert
            name.Should().Be(_defaultCompanyName);
        }

        [Test]
        public void StartRent_Rent_Existing_Scooter_ScooterIsRented()
        {
            //Arrange
            var scooter = new Scooter("1", 0.2m);
            _scooterServiceMock.Setup(s => s.GetScooterById("1")).Returns(scooter);

            //Act
            _company.StartRent("1");

            //Assert
            scooter.IsRented.Should().BeTrue();
        }

        [Test]
        public void StartRent_Rent_Already_Rented_Scooter_Throws_ScooterIsAlreadyRentedException()
        {
            //Arrange
            var scooter = new Scooter("1", 0.2m) { IsRented = true };
            _scooterServiceMock.Setup(s => s.GetScooterById("1")).Returns(scooter);

            //Act
            Action action = () => _company.StartRent("1");

            //Assert
            action.Should().Throw<ScooterIsAlreadyRentedException>();
        }

        [Test]
        public void StartRent_Rent_NonExisting_Scooter__Throws_ScooterNotFoundException()
        {
            //Act
            Action action = () => _company.StartRent("1");

            //Assert
            action.Should().Throw<ScooterNotFoundException>().WithMessage("Scooter with provided id doesn't exist.");
        }

        [Test]
        public void EndRent_StopRenting_Existing_Scooter_ScooterRentStopped()
        {
            //Arrange
            var scooter = new Scooter("1", 0.2m) { IsRented = true };
            var now = DateTime.Now;
            var rentalRecord = new RentedScooter(scooter.Id, now.AddMinutes(-20), scooter.PricePerMinute) { RentEnd = now };
            _rentedScooterArchiveMock.Setup(archive => archive.EndRental(scooter.Id, It.IsAny<DateTime>())).Returns(rentalRecord);
            _scooterServiceMock.Setup(s => s.GetScooterById("1")).Returns(scooter);
            _rentalCalculatorMock.Setup(calculator => calculator.CalculateRent(rentalRecord)).Returns(4); ;

            //Act
            var result = _company.EndRent("1");

            //Assert
            scooter.IsRented.Should().BeFalse();
            result.Should().Be(4);
        }

        [Test]
        public void EndRent_StopRenting_NonExisting_Scooter_Throws_ScooterNotFoundException()
        {
            //Act
            Action action = () => _company.EndRent("1");

            //Assert
            action.Should().Throw<ScooterNotFoundException>();
        }

        [Test]
        public void CalculateIncome_Should_Returns_Income()
        {
            //Arrange
            _rentalCalculatorMock.Setup(calculator => calculator.CalculateIncome(It.IsAny<int?>(), It.IsAny<bool>()))
                .Returns(100);

            //Act
            var result = _company.CalculateIncome(null, true);

            //Assert
            result.Should().Be(100);
        }
    }
}
