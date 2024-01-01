using Data.Context;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories 
{
    public class TicketDbRepository : ITicketRepository 
    {

        private AirlineDbContext _AirlineContext;


        public TicketDbRepository(AirlineDbContext Context)
        {
            _AirlineContext = Context;
        }

        public void Book(Ticket ticket)
        {

            var TicketBooked = _AirlineContext.Ticket.Any(t => t.Row == ticket.Row &&
                                                    t.Column == ticket.Column &&
                                                    t.FlightIdFk == ticket.FlightIdFk &&
                                                    !t.Cancelled);

            if (!TicketBooked)
            {
                _AirlineContext.Ticket.Add(ticket);
                _AirlineContext.SaveChanges();
            }
        }

        public void cancel(Ticket ticket)
        {
            var existingTicket = _AirlineContext.Ticket.FirstOrDefault(t => t.Id == ticket.Id);

            if (existingTicket != null)
            {
                existingTicket.Cancelled = true;
                _AirlineContext.SaveChanges();
            }
        }

        public IQueryable<Ticket> GetTickets()
        {
            return _AirlineContext.Ticket;
        }
        public IQueryable<Ticket> GetTicketInfo(int ticketId)
        {
            return _AirlineContext.Ticket.Where(t => t.Id == ticketId);
        }

        public IQueryable<Ticket> GetFlightTickets(int FlightId)
        {
            return _AirlineContext.Ticket.Where(t => t.FlightIdFk == FlightId);

        }


        //-----------------
        //Modifications

        public int GetSeatsBooked(int flightId)
        {
            return _AirlineContext.Ticket.Count(t => t.FlightIdFk == flightId && !t.Cancelled);
        }
    }
}
