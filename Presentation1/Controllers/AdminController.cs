using Data.Context;
using Data.Repositories;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models.ViewModel;
using System.Net.Sockets;

namespace Presentation.Controllers
{
    public class AdminController : Controller
    {
        private ITicketRepository _ticketRepository;
        private FlightDbRepository _flightRepository;

        public AdminController(ITicketRepository ticketRepository,
            FlightDbRepository flightRepository)
        {

            _ticketRepository = ticketRepository;
            _flightRepository = flightRepository;

        }

        public IActionResult Index()
        {
            IQueryable<Flight> FlightList = _flightRepository.getFlights();

            var output = from p in FlightList
                         select new FlightListViewModel()
                         {
                             Id = p.Id,
                             Seats = (p.Rows * p.Columns),
                             DepartureDate = p.DepartureDate,
                             Arrivaldate = p.Arrivaldate,
                             CountryFrom = p.CountryFrom,
                             CountryTo = p.CountryTo,
                             price = (p.WholesalePrice + (p.WholesalePrice * p.CommissionRate))
                         };

            return View(output);
        }

        public IActionResult Tickets(int flightId)
        {
            var tickets = _ticketRepository.GetFlightTickets(flightId);

            var output = from p in tickets
                         select new TicketViewModel()
                         {
                             Id = p.Id,
                            Seat = (p.Row.ToString() + p.Column),
                            Passport = p.Passport
                            // Set other properties as needed
                        };
            return View(output);
        }

        public IActionResult TicketDetails(int ticketId)
        {
            var tickets = _ticketRepository.GetTickets();
            var flightIds = tickets.Where(t => t.Id == ticketId).Select(t => t.FlightIdFk).FirstOrDefault();

            // Assuming _flightRepository.getFlights() returns a list of Flight objects
            var flight = _flightRepository.getFlights().FirstOrDefault(f => f.Id == flightIds);

            var ticketDetail = (from ticket in tickets
                                where ticket.Id == ticketId
                                select new TicketViewModel
                                {
                                    //Ticket info
                                    FlightId = ticket.FlightIdFk,
                                    Id = ticket.Id,
                                    Seat = (ticket.Row.ToString() + ticket.Column),
                                    Passport = ticket.Passport,
                                    PricePaid = ticket.PricePaid,
                                    Cancelled = ticket.Cancelled,
                                    image = ticket.Image,
                                    // Flight info
                                    DepartureDate = flight.DepartureDate.ToString("yyyy-MM-dd HH:mm"),
                                    ArrivalDate = flight.Arrivaldate.ToString("yyyy-MM-dd HH:mm"),
                                    CountryFrom = flight.CountryFrom,
                                    CountryTo = flight.CountryTo,
                                }).FirstOrDefault();

            if (ticketDetail == null)
            {
                TempData["error"] = "No ticket with that id";
                return NotFound();
            }

            return View(ticketDetail);
        
        }

    }
}

