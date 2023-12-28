using Data.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models.ViewModel;

namespace Presentation.Controllers
{
    public class TicketsController : Controller
    {

        private readonly TicketDbRepository _ticketRepository;
        private readonly FlightDbRepository _flightRepository;

        public TicketsController(TicketDbRepository ticketRepository, FlightDbRepository flightRepository)
        {
            
            _ticketRepository = ticketRepository;
            _flightRepository = flightRepository;
        }
        
        public IActionResult Index()
        {
            var CurrentTime = DateTime.Now;

            IQueryable<Flight> FlightList = _flightRepository.getFlights().Where(f =>f.DepartureDate >= CurrentTime);

            var output = from p in FlightList
                         select new FlightListViewModel()
                         {
                             Id = p.Id,
                             Rows = p.Rows,
                             Columns = p.Columns,
                             DepartureDate = p.DepartureDate,
                             Arrivaldate = p.Arrivaldate,
                             CountryFrom = p.CountryFrom,
                             CountryTo = p.CountryTo,
                             price = p.WholesalePrice
                         };


            return View("Index", output);
        }
    }
}
