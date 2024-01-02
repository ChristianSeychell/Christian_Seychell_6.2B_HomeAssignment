using Data.Context;
using Data.Repositories;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Hosting;
using Presentation.Models.ViewModel;
using System.Collections.Generic;
using System.Net.Sockets;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Presentation.Controllers
{
    
        public class TicketsController : Controller
        {

            private ITicketRepository _ticketRepository;
            private FlightDbRepository _flightRepository;
            private AirlineDbContext _airlineDbContext;
                private UserManager<ApplicationUser> _userManager;

            public TicketsController(ITicketRepository ticketRepository,
                FlightDbRepository flightRepository, AirlineDbContext airlineDbContext, UserManager<ApplicationUser> userManager)
            {

                _ticketRepository = ticketRepository;
                _flightRepository = flightRepository;
                _airlineDbContext = airlineDbContext;
                _userManager = userManager;

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
        [AllowAnonymous]
        public async Task<IActionResult> BookAsync(int flightId, decimal pricePaid, int seats)
            {
                var flight = _flightRepository.GetFlight(flightId);
                int maxRows = flight.Rows;
                int maxCols = flight.Columns;

                var user = await _userManager.GetUserAsync(User);

            if (User.Identity.IsAuthenticated)
            {
               
                    BookViewModel viewModel = new BookViewModel
                    {

                        FlightId = flightId,
                        PricePaid = pricePaid,
                        Maxrows = maxRows,
                        Maxcols = maxCols,
                        Passport = user.Passport
                    };
                    return View(viewModel);


            }
            else
            {
                BookViewModel viewModel = new BookViewModel
                {

                    FlightId = flightId,
                    PricePaid = pricePaid,
                    Maxrows = maxRows,
                    Maxcols = maxCols,
                };
                return View(viewModel);
            }

            }

            [HttpPost]
            public IActionResult Book(BookViewModel b, [FromServices] IWebHostEnvironment host)
            {
                if (ModelState.IsValid)
                {
                    var TicketBooked = _ticketRepository.GetTickets().FirstOrDefault(t => t.Row == b.Row &&
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

                if (TicketBooked == null)
                    {
                        var flight = _flightRepository.GetFlight(b.FlightId);

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
                    
                   TempData["error"] = "Ticket is already booked";       
                    
                }


            }
                return View(b);
            }

            [Authorize]
            public async Task<IActionResult> TicketsAsync()
            {

            var user = await _userManager.GetUserAsync(User);
            List<Ticket> list = _ticketRepository.GetTickets().Where(t => t.Passport == user.Passport).ToList();
                var output = from t in list
                             select new TicketViewModel()
                             {
                                 Id = t.Id,
                                 Seat = t.Column + t.Row.ToString(),
                                 FlightId = t.FlightIdFk,
                                 Passport = t.Passport,
                                 PricePaid = t.PricePaid,
                                 Cancelled = t.Cancelled

                             };
                return View(output);
            }

            public IActionResult Cancel(int ticketId)
            {
                var ticket = _ticketRepository.GetTicketInfo(ticketId).FirstOrDefault();

                _ticketRepository.cancel(ticket);

                return RedirectToAction("Index");
            }

        public IActionResult CheckSeatAailable(int FlightId, int row, string column)
        {
            var TicketBooked = _ticketRepository.GetTickets().FirstOrDefault(t => t.Row == row &&
                                                                        t.Column == column &&
                                                                       t.FlightIdFk == FlightId &&
                                                                        !t.Cancelled);
            if (TicketBooked == null)
            {
                return Json(true);
            }
            else
            {
                return Json("The seat is Taken");
            }

        }
    }
}