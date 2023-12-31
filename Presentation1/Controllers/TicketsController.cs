using Data.Context;
using Data.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Hosting;
using Presentation.Models.ViewModel;
using System.Net.Sockets;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Presentation.Controllers
{
    public class TicketsController : Controller
    {

        private TicketDbRepository _ticketRepository;
        private FlightDbRepository _flightRepository;
        private AirlineDbContext _airlineDbContext;

        public TicketsController(TicketDbRepository ticketRepository,
            FlightDbRepository flightRepository, AirlineDbContext airlineDbContext)
        {

            _ticketRepository = ticketRepository;
            _flightRepository = flightRepository;
            _airlineDbContext = airlineDbContext;

        }

        public IActionResult Index()
        {
            var CurrentTime = DateTime.Now;

            IQueryable<Flight> FlightList = _flightRepository.getFlights().Where(f => f.DepartureDate >= CurrentTime);
            

            var output = from p in FlightList
                         select new FlightListViewModel()
                         {
                             Id = p.Id,
                             Seats = (p.Rows * p.Columns) - _ticketRepository.GetSeatsBooked(p.Id),
                             DepartureDate = p.DepartureDate,
                             Arrivaldate = p.Arrivaldate,
                             CountryFrom = p.CountryFrom,
                             CountryTo = p.CountryTo,
                             price = (p.WholesalePrice + (p.WholesalePrice * p.CommissionRate))
                         };



            return View(output);
        }

        [HttpGet]
        public IActionResult Book(int flightId,decimal pricePaid)
        {
            BookViewModel viewModel;

            if (User.Identity.IsAuthenticated)
            {
                viewModel = new BookViewModel
                {

                    FlightId = flightId,
                    PricePaid = pricePaid,
                   // Passport = _ticketRepository.GetTicket(id)
                };
            }
            else
            {
                viewModel = new BookViewModel
                {

                    FlightId = flightId,
                    PricePaid = pricePaid
                };
            }
            return View(viewModel);
        }



        [HttpPost]
        public IActionResult Book(BookViewModel b, [FromServices] IWebHostEnvironment host)
        {
            if (ModelState.IsValid)
            {
                var TicketBooked = _airlineDbContext.Ticket.Any(t => t.Row == b.Row &&
                                                                     t.Column == b.Column &&
                                                                    t.FlightIdFk == b.FlightId &&
                                                                     !t.Cancelled);
                string relativePath = "";
                //upload of an image
                if (b.imageFile != null)
                {
                    string newFilename = Guid.NewGuid().ToString()
                        + Path.GetExtension(b.imageFile.FileName);

                    relativePath = "/images/" + newFilename;
                    string absolutePath = host.WebRootPath + "\\images\\" + newFilename;

                    using (FileStream fs = new FileStream(absolutePath, FileMode.CreateNew))
                    {
                        b.imageFile.CopyTo(fs);
                        fs.Flush();
                    }
                }

                if (!TicketBooked)
                {
                    var flight  = _flightRepository.GetFlight(b.FlightId);

                    var currentDateAndTime = DateTime.Now;
                    if (flight != null)
                    {
                        if (flight.DepartureDate > currentDateAndTime)
                        {
                            {
                                // Create a Ticket object from the ViewModel
                                var ticket = new Ticket
                                {
                                    Row = b.Row,
                                    Column = b.Column,
                                    Passport = b.Passport,
                                    PricePaid = b.PricePaid,
                                    FlightIdFk = b.FlightId,
                                    Cancelled = false,
                                    Image = relativePath
                                    // Set other properties as needed
                                };

                                // Use your TicketDbRepository to book the ticket
                                _ticketRepository.Book(ticket);

                                // Redirect to a confirmation page or display a success message
                                return RedirectToAction("index");
                            }
                        }
                    }
                }
                else
                {
                    b.cancelled = false;
                    _airlineDbContext.SaveChanges();
                }
            }
            return View(b);
        }
        //[Authorize]
        public IActionResult Tickets(string passport)
        {
            IQueryable<Ticket> list = _ticketRepository.GetTickets().Where(t => t.Passport == passport);
            var output = from t in list
                         select new TicketViewModel()
                         {
                             Id = t.Id,
                             Seat = t.Column + t.Row.ToString(),
                             FlightId = t.FlightIdFk,
                             Passport = t.Passport,
                             PricePaid = t.PricePaid,
                             Cancelled= t.Cancelled

                         };
        return View(output);
        }

        public IActionResult Cancel(int ticketId)
        {
            var ticket = _ticketRepository.GetTicketInfo(ticketId).FirstOrDefault();

            if (ticket == null)
            {
                // TEmp data
                return NotFound();
            }

            if (ticket.Cancelled)
            {
                return RedirectToAction("Tickets");
            }

            _ticketRepository.cancel(ticket);

            return RedirectToAction("Index");
        }
    }
}