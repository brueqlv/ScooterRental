
using ScooterRental.Exceptions;

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
            if (string.IsNullOrEmpty(id))
            {
                throw new InvalidIdException();
            }

            if (pricePerMinute <= 0)
            {
                throw new InvalidPriceException();
            }

            if (_scooters.Any(scooter => scooter.Id == id))
            {
                throw new DuplicateScooterException();
            }

            _scooters.Add(new Scooter(id, pricePerMinute));
        }

        public Scooter GetScooterById(string scooterId)
        {
            var scooter = GetValidScooterById(scooterId);

            return scooter;
        }

        public IList<Scooter> GetScooters()
        {
            return _scooters.ToList();
        }

        public void RemoveScooter(string id)
        {
            var scooter = GetValidScooterById(id);

            if (scooter.IsRented)
            {
                throw new CannotRemoveRentedScooterException();
            }

            _scooters.Remove(scooter);
        }

        private Scooter GetValidScooterById(string id)
        {
            var scooter = _scooters.SingleOrDefault(scooter => scooter.Id.Equals(id));

            return scooter ?? throw new ScooterNotFoundException("Scooter with provided id doesn't exist.");
        }
    }
}
