namespace ScooterRental.Interfaces
{
    public interface IRentalCalculatorService
    {
        decimal CalculateIncome(int? year, bool includeNotCompletedRentals);
        decimal CalculateRent(RentedScooter rentalRecord);
    }
}
