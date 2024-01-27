
using FluentAssertions;

namespace ScooterRental.Tests
{
    public class RentedScooterTests
    {

        private string _id = "1";
        private DateTime _startDate = new DateTime(2024, 01, 01, 00, 00, 00);
        private decimal _pricePerMinute = 0.2m;

        [Test]
        public void Constructor_Should_Set_ScooterId_RentStart_PricePerMinute_Correctly()
        {
            //Arrange
            var rentedScooter = new RentedScooter(_id, _startDate, _pricePerMinute);

            //Assert
            rentedScooter.ScooterId.Should().Be(_id);
            rentedScooter.RentStart.Should().Be(_startDate);
            rentedScooter.PricePerMinute.Should().Be(_pricePerMinute);
        }

        [Test]
        public void Property_RentEnd_Should_Set_RentEnd_Correctly()
        {
            //Arrange
            DateTime rentEnd = new DateTime(2024, 01, 02, 10, 00, 00);
            var rentedScooter = new RentedScooter(_id, _startDate, _pricePerMinute);

            //Act
            rentedScooter.RentEnd = rentEnd;

            //Assert
            rentedScooter.RentEnd.Should().Be(rentEnd);
        }
    }
}
