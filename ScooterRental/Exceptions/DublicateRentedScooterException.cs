using System.Runtime.Serialization;

namespace ScooterRental.Exceptions
{
    [Serializable]
    public class DublicateRentedScooterException : Exception
    {
        public DublicateRentedScooterException() : base("Provided rented scooter entry already exists in the archive")
        {
        }
    }
}