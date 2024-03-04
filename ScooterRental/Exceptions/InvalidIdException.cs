

namespace ScooterRental.Exceptions
{
    [Serializable]
    public class InvalidIdException : Exception
    {
        public InvalidIdException() : base("Provided invalid id.")
        {
        }
    }
}