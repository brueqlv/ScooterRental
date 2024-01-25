
using FluentAssertions;

namespace ScooterRental.Tests
{
    public class ScooterTests
    {
        private Scooter? _scooter;

        [Test]
        public void Constructor_Sets_Id_And_PricePerMinute_IsRented_Correctly()
        {
            //Arrange
            _scooter = new Scooter("1", 0.2m);

            //Assert
            _scooter.Id.Should().Be("1");
            _scooter.PricePerMinute.Should().Be(0.2m);
            _scooter.IsRented.Should().BeFalse();
        }
    }
}
