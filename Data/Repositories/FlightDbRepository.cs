using Data.Context;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class FlightDbRepository
    {
        private AirlineDbContext _AirlineContext;

        public FlightDbRepository(AirlineDbContext Context)
        {
            _AirlineContext = Context;
        }

        public Flight? GetFlight(int id)
        {
            return getFlights().SingleOrDefault(f => f.Id == id);
        }

        public IQueryable<Flight> getFlights()
        {
            return _AirlineContext.flight;
        }
    }
}
