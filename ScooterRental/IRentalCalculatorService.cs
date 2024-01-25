
namespace ScooterRental
{
    public interface IRentalCalculatorService
    {
        decimal CalculateIncome(int? year, bool includeNotCompletedRentals);
        decimal CalculateRent(RentedScooter rentalRecord);
    }
}
