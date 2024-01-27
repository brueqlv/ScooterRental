using ScooterRental.Exceptions;
using ScooterRental.Interfaces;

namespace ScooterRental
{
    public class RentalCalculatorService : IRentalCalculatorService
    {
        private readonly decimal _maxDailyPrice = 20m;
        private readonly IRentedScooterArchive _scooterArchive;
        private readonly List<RentedScooter> _scooterList;

        public RentalCalculatorService(IRentedScooterArchive scooterArchive)
        {
            _scooterArchive = scooterArchive;
            _scooterList = _scooterArchive.GetAllRentedScooterList();
        }

        public decimal CalculateIncome(int? year, bool includeNotCompletedRentals)
        {
            if(year > DateTime.Now.Year)
            {
                throw new InvalidYearException();
            }

            decimal income = 0m;

            if(year == null)
            {
                foreach(var rentedScooter in _scooterList)
                {
                    if (includeNotCompletedRentals)
                    {
                        income += CalculateRent(rentedScooter);
                    }
                    else
                    {
                        if(rentedScooter.RentEnd != null)
                        {
                            income += CalculateRent(rentedScooter); 
                        }
                    }
                }
            }
            else
            {
                foreach(var rentedScooter in _scooterList)
                {
                    if( rentedScooter.RentEnd == null && DateTime.Now.Year == year)
                    {
                        if (includeNotCompletedRentals)
                        {
                            income += CalculateRent(rentedScooter);
                        }
                    }
                    else if(rentedScooter.RentEnd != null && ((DateTime)rentedScooter.RentEnd).Year == year)
                    {
                        income += CalculateRent(rentedScooter);
                    }
                }
            }

            return income;
        }

        public decimal CalculateRent(RentedScooter rentalRecord)
        {
            DateTime rentEndTime = DateTime.Now;
            DateTime rentStartTime = rentalRecord.RentStart;
            decimal totalRent = 0m;

            if (rentalRecord.RentEnd != null)
            {
                rentEndTime = (DateTime)rentalRecord.RentEnd;
            }

            DateTime currentDay = rentalRecord.RentStart.Date;

            while(currentDay <= rentEndTime.Date)
            {
                DateTime endOfCurrentDay = currentDay.Date.AddDays(1);
                DateTime dayEndTime = new DateTime();

                if(currentDay == rentEndTime.Date)
                {
                    dayEndTime = rentEndTime;
                }
                else
                {
                    dayEndTime = endOfCurrentDay;
                }

                int currentDayMinutes = (int)(dayEndTime - rentStartTime).TotalMinutes;
                totalRent += GetDaysRent(currentDayMinutes, rentalRecord.PricePerMinute, _maxDailyPrice);

                currentDay = endOfCurrentDay;
                rentStartTime = endOfCurrentDay;
            }

            return totalRent;
        }

        private static decimal GetDaysRent(int minutes, decimal pricePerMinute, decimal maxDailyPrice)
        {
            return Math.Min(minutes * pricePerMinute, maxDailyPrice);
        }
    }
}
