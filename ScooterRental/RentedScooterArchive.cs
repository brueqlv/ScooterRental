
using ScooterRental.Exceptions;

namespace ScooterRental
{
    public class RentedScooterArchive : IRentedScooterArchive
    {
        private readonly List<RentedScooter> _rentedScooterList;

        public RentedScooterArchive(List<RentedScooter> rentedScooterList)
        {
            _rentedScooterList = rentedScooterList;
        }

        public void AddRentedScooter(RentedScooter scooter)
        {
            if (_rentedScooterList.Any(existingScooter =>
                existingScooter.ScooterId == scooter.ScooterId &&
                existingScooter.RentStart == scooter.RentStart))
            {
                throw new DublicateRentedScooterException();
            }

            _rentedScooterList.Add(scooter);
        }

        public RentedScooter EndRental(string scooterId, DateTime rentEnd)
        {
            var lastRentedScooter = _rentedScooterList
                                    .Where(scooter => scooter.ScooterId == scooterId)
                                    .LastOrDefault();

            if (lastRentedScooter == null)
            {
                throw new ScooterNotFoundException("Scooter with provided id doesn't exist.");
            }

            if (lastRentedScooter.RentEnd != null)
            {
                throw new ScooterNotFoundException("Scooter with provided id has already rent end time.");
            }

            if (rentEnd < lastRentedScooter.RentStart)
            {
                throw new InvalidDateException();
            }

            lastRentedScooter.RentEnd = rentEnd;

            return lastRentedScooter;
        }

        public List<RentedScooter> GetAllRentedScooterList()
        {
            return _rentedScooterList;
        }
    }
}
