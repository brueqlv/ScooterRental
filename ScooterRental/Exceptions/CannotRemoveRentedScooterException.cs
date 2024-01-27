
namespace ScooterRental.Exceptions
{
    [Serializable]
    public class CannotRemoveRentedScooterException : Exception
    {
        public CannotRemoveRentedScooterException() : base("Scooter with provided id is still rented.")
        {
        }
    }
}