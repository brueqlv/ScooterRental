
using ScooterRental.Exceptions;
using System.Linq;

namespace ScooterRental
{
    public class ScooterService : IScooterService
    {
        private readonly List<Scooter> _scooters;

        public ScooterService(List<Scooter> scooters)
        {
            _scooters = scooters;
        }

        public void AddScooter(string id, decimal pricePerMinute)
        {
            if(string.IsNullOrEmpty(id))
            {
                throw new InvalidIdException();
            }

            if(pricePerMinute <= 0)
            {
                throw new InvalidPriceException();
            }

            if(_scooters.Any(scooter => scooter.Id == id))
            {
                throw new DublicateScooterException();
            }

            _scooters.Add(new Scooter(id, pricePerMinute));
        }

        public Scooter GetScooterById(string scooterId)
        {
            if (scooterId == null || scooterId.Length == 0)
            {
                throw new InvalidIdException();
            }

            var scooter = _scooters.SingleOrDefault(scooter => scooter.Id.Equals(scooterId));

            if (scooter == null)
            {
                throw new ScooterNotFoundException();
            }

            return scooter;
        }

        public IList<Scooter> GetScooters()
        {
            return _scooters.ToList();
        }

        public void RemoveScooter(string id)
        {
            if(id == null || id.Length == 0)
            {
                throw new InvalidIdException();
            }

            var scooter = _scooters.SingleOrDefault(scooter => scooter.Id.Equals(id));

            if(scooter == null)
            {
                throw new ScooterNotFoundException();
            }

            _scooters.Remove(scooter);
        }
    }
}
