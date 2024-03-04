using System.Runtime.Serialization;

namespace ScooterRental.Exceptions
{
    [Serializable]
    public class ScooterNotFoundException : Exception
    {
        public ScooterNotFoundException(string v) : base(v)
        {
        }
    }
}