
using FluentAssertions;
using ScooterRental.Exceptions;

namespace ScooterRental.Tests
{
    public class ScooterServiceTests
    {
        private IScooterService _scooterService;
        private List<Scooter> _scooters;
        private const string _defaultStooterId = "1";

        [SetUp]
        public void Setup()
        {
            _scooters = new List<Scooter>();
            _scooterService = new ScooterService(_scooters);
        }

        [Test]
        public void AddScooter_Valid_Data_Provided_ScooterAdded()
        {
            //Act
            _scooterService.AddScooter(_defaultStooterId, 0.1m);

            //Assert
            _scooters.Count.Should().Be(1);
        }

        [Test]
        public void AddScooter_Invalid_Price_Provided_InvalidPriceException_Expected()
        {
            //Act
            Action action = () => _scooterService.AddScooter(_defaultStooterId, 0.0m);

            //Assert
            action.Should().Throw<InvalidPriceException>();
        }

        [Test]
        public void AddScooter_Invalid_Id_Provided_InvalidPIdException_Expected()
        {
            //Act
            Action action = () => _scooterService.AddScooter("", 0.0m);

            //Assert
            action.Should().Throw<InvalidIdException>();
        }

        [Test]
        public void AddScooter_Add_Dublicate_Scooter_DublicateScooterException_Expected()
        {
            //Arrange
            _scooters.Add(new Scooter(_defaultStooterId, 0.1m));

            //Act
            Action action = () => _scooterService.AddScooter(_defaultStooterId, 0.2m);

            //Assert
            action.Should().Throw<DublicateScooterException>();
        }

        [Test]
        public void RemoveScooter_Existing_Id_Provided_Scooter_Removed()
        {
            //Arrange
            _scooters.Add(new Scooter(_defaultStooterId, 0.1m));

            //Act
            _scooterService.RemoveScooter(_defaultStooterId);

            //Assert
            _scooters.Count.Should().Be(0);
        }

        [Test]
        public void RemoveScooter_NonExisting_Id_Provided_Scooter_ScooterNotFoundException()
        {
            //Act
            Action action = () => _scooterService.RemoveScooter(_defaultStooterId); 

            //Assert
            action.Should().Throw<ScooterNotFoundException>();
        }

        [Test]
        public void RemoveScooter_Invalid_Id_Provided_Scooter_InvalidIdException()
        {
            //Act
            Action action = () => _scooterService.RemoveScooter(_defaultStooterId);

            //Assert
            action.Should().Throw<ScooterNotFoundException>();
        }

        [Test]
        public void GetScooters_Returns_Scooter_List()
        {
            //Arrange
            var testScooterList = new List<Scooter>();
            testScooterList.Add(new Scooter(_defaultStooterId, 0.2m));
            testScooterList.Add(new Scooter("2", 0.2m));

            //Act
            _scooterService.AddScooter(_defaultStooterId, 0.2m);
            _scooterService.AddScooter("2", 0.2m);

            //Assert
            testScooterList.Should().BeEquivalentTo(_scooterService.GetScooters());
        }

        [Test]
        public void GetScooterById_Valid_Id_Returns_Scooter()
        {
            //Arrange
            _scooterService.AddScooter(_defaultStooterId, 0.2m);

            //Act
            var scooter = _scooterService.GetScooterById(_defaultStooterId);

            //Assert
            scooter.Should().BeEquivalentTo(new Scooter(_defaultStooterId, 0.2m));
        }

        [Test]
        public void GetScooterById_NonExisting_Id_Throws_ScooterNotFoundException()
        {
            //Act
            Action action = () => _scooterService.GetScooterById("2");

            //Assert
            action.Should().Throw<ScooterNotFoundException>();
        }
    }
}
