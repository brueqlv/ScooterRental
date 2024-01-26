
using FluentAssertions;
using ScooterRental.Exceptions;

namespace ScooterRental.Tests
{
    public class RentedScooterArchiveTests
    {
        private List<RentedScooter> _rentedScooters;
        private RentedScooterArchive _archive;

        [SetUp]
        public void Setup()
        {
            _rentedScooters = new List<RentedScooter>
            {
                new RentedScooter("1", new DateTime(2023, 01, 23, 10, 00, 00), 0.2m),
                new RentedScooter("2", new DateTime(2023, 02, 20, 12, 00, 00), 0.1m),
                new RentedScooter("3", new DateTime(2023, 03, 13, 14, 00, 00), 0.3m),
            };

            _archive = new RentedScooterArchive(_rentedScooters);
        }

        [Test]
        public void AddRentedScooter_Adds_Rented_Scooter_To_List()
        {
            //Act
            _archive.AddRentedScooter( new RentedScooter("1", new DateTime(2023, 04, 13, 14, 00, 00), 0.3m));
            _archive.AddRentedScooter(new RentedScooter("1", new DateTime(2023, 04, 14, 14, 00, 00), 0.3m));

            //Assert
            _rentedScooters.Count.Should().Be(5);
        }

        [Test]
        public void AddRentedScooter_Add_Dublicate_Entry_Throws_DublicateRentedScooterException()
        {
            //Arrange
            _archive.AddRentedScooter(new RentedScooter("1", new DateTime(2023, 04, 13, 14, 00, 00), 0.3m));

            //Act
            Action action = () => _archive.AddRentedScooter(new RentedScooter("1", new DateTime(2023, 04, 13, 14, 00, 00), 0.3m));

            //Assert
            action.Should().Throw<DublicateRentedScooterException>();
        }

        [Test]
        public void EndRental_Provided_ExistingId_And_Valid_Date_Should_Update_EndTime()
        {
            //Act
            _archive.EndRental("1", new DateTime(2023, 01, 25, 10, 00, 00));

            //Assert
            _rentedScooters.FirstOrDefault(scooter => scooter.ScooterId == "1").RentEnd.Should().Be(new DateTime(2023, 01, 25, 10, 00, 00));
        }

        [Test]
        public void EndRental_Provided_NonexistingId_And_Valid_Date_Should_Throw_ScooterNotFoundException()
        {
            //Act
            Action action = () => _archive.EndRental("5", new DateTime(2023, 01, 25, 10, 00, 00));

            //Assert
            action.Should().Throw<ScooterNotFoundException>().WithMessage("Scooter with provided id doesn't exist.");
        }

        [Test]
        public void EndRental_Provided_ExistingId_And_Invalid_Date_Should_Throw_InvalidDateException()
        {
            //Act
            Action action = () => _archive.EndRental("1", new DateTime(2022, 01, 25, 10, 00, 00));

            //Assert
            action.Should().Throw<InvalidDateException>();
        }

        [Test]
        public void EndRental_Provided_ExistingId_Scooter_Already_Ended_Should_Throw_ScooterNotFoundException()
        {
            //Arrange
            _archive.EndRental("1", new DateTime(2023, 01, 23, 15, 00, 00));

            //Act
            Action action2 = () => _archive.EndRental("1", new DateTime(2023, 01, 23, 15, 00, 00));

            //Assert
            action2.Should().Throw<ScooterNotFoundException>().WithMessage("Scooter with provided id has already rent end time.");
        }

        [Test]
        public void GetAllRentedScooterList_Should_Return_Correct_List()
        {
            //Act
            var scooterList = _archive.GetAllRentedScooterList();

            //Assert
            _rentedScooters.Should().BeEquivalentTo(scooterList);
        }
    }
}
