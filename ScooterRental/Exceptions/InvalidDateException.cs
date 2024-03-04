using System.Runtime.Serialization;

namespace ScooterRental.Exceptions
{
    [Serializable]
    public class InvalidDateException : Exception
    {
        public InvalidDateException() : base("Provided date is not valid.")
        {
        }
    }
}