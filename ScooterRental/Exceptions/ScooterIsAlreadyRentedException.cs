using System.Runtime.Serialization;

namespace ScooterRental.Exceptions
{
    [Serializable]
    public class ScooterIsAlreadyRentedException : Exception
    {
        public ScooterIsAlreadyRentedException() : base("Scooter with provided id is already rented.")
        {
        }
    }
}