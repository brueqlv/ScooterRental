using System.Runtime.Serialization;

namespace ScooterRental.Exceptions
{
    [Serializable]
    public class InvalidYearException : Exception
    {
        public InvalidYearException() :base("Provided invalid year.")
        {
        }
    }
}