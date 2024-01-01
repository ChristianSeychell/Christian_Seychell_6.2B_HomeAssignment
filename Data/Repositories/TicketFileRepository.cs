using Data.Context;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class TicketFileRepository : ITicketRepository
    {

        private AirlineDbContext _AirlineContext;
        private FlightDbRepository _flightRepository;


        public TicketFileRepository(AirlineDbContext Context, FlightDbRepository flightRepository)
        {
            _AirlineContext = Context;
            _flightRepository = flightRepository;
        }

        string filePath;
        public TicketFileRepository(string pathFile)
        {
            filePath = pathFile;

            if (System.IO.File.Exists(filePath) == false)
            {

                using (var myFile = System.IO.File.Create(filePath))
                {
                    myFile.Close(); 
                }
            }


        }

        public void Book(Ticket ticket)
        {
            var time = DateTime.Now;

            var TicketBooked = GetTickets().Where(t => t.Row == ticket.Row &&
                                          t.Column == ticket.Column &&
                                          t.FlightIdFk == ticket.FlightIdFk &&
                                          !t.Cancelled).ToList();

            if (!TicketBooked.Any())
            {
               /* var flight = _flightRepository.GetFlight(ticket.FlightIdFk);

                if (flight != null)
                {*/
                   /* if (flight.DepartureDate > time)
                    {*/


                        var allTickets = GetTickets().ToList();
                        var id = allTickets.Count > 0 ? allTickets.Max(t => t.Id) + 1 : 1;
                        ticket.Id = id;

                        allTickets.Add(ticket);

                        string jsonString = JsonSerializer.Serialize(allTickets);

                        System.IO.File.WriteAllText(filePath, jsonString);
                    //}
                //}
            }
        }

        public void cancel(Ticket ticket)
        { 
         var AllTickets = GetTickets().ToList();

          var ticketToCancel = AllTickets.FirstOrDefault(t => t.Id == ticket.Id);

            if (ticketToCancel != null)
            {
                ticketToCancel.Cancelled = true;

                string jsonString = JsonSerializer.Serialize(AllTickets);

                System.IO.File.WriteAllText(filePath, jsonString);
            }
            else
            {
                //temp data
            }


        }


            public IQueryable<Ticket> GetTickets() 
        {
            string allText = System.IO.File.ReadAllText(filePath);

            if (allText == null)
            {
                return new List<Ticket>().AsQueryable();
            }
            else
            {
                try
                {
                    List<Ticket> tickets = JsonSerializer.Deserialize<List<Ticket>>(allText);
                    return tickets.AsQueryable();
                }
                catch
                {
                    return new List<Ticket>().AsQueryable();
                }
            }
        
        }

        public IQueryable<Ticket> GetTicketInfo(int ticketId)
        {
            var AllTickets = GetTickets().AsQueryable();
            var Ticket = AllTickets.Where(t => t.Id == ticketId);

            return Ticket;
        }

        public int GetSeatsBooked(int flightId)
        {
            var AllTickets = GetTickets().ToList();
            var seatbooked = AllTickets.Count(t => t.FlightIdFk == flightId && !t.Cancelled);

            return seatbooked;
        }

        public IQueryable<Ticket> GetFlightTickets(int FlightId)
        {

            var AllTickets = GetTickets().AsQueryable();
            var TicketsPerFLight = AllTickets.Where(t => t.FlightIdFk == FlightId);
            return TicketsPerFLight;

        }
    }
}
