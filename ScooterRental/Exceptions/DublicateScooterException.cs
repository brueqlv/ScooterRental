
namespace ScooterRental.Exceptions
{
    public class DublicateScooterException : Exception
    {
        public DublicateScooterException() : base("Scooter with provided id exists.")
        { }
    }
}
