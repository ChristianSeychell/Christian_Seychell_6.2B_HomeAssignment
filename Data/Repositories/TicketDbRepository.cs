using Data.Context;
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
    public class TicketDbRepository
    {

        private AirlineDbContext _Context;

        public TicketDbRepository(AirlineDbContext Context)
        {
            _Context = Context;
        }

        public void Book(Ticket ticket)
        {

            var TicketBooked = _Context.Ticket.Any(t => t.Row == ticket.Row &&
                                                    t.Column == ticket.Column &&
                                                    t.FlightIdFk == ticket.FlightIdFk &&
                                                    !t.Cancelled);

            if (!TicketBooked)
            {
                _Context.Ticket.Add(ticket);
                _Context.SaveChanges();
            }
        }

        public void cancel(Ticket ticket)
        {
            var existingTicket = _Context.Ticket.FirstOrDefault(t => t.Id == ticket.Id);

            if (existingTicket != null)
            {
                existingTicket.Cancelled = true;
                _Context.SaveChanges();
            }
        }

        public IQueryable<Ticket> GetTickets()
        {
            return _Context.Ticket;
        }
        public IQueryable<Ticket> GetTicketInfo(int ticketId)
        {
            return _Context.Ticket.Where(t => t.Id == ticketId);
        }

        public IQueryable<Ticket> GetFlightTickets(int FlightId)
        {
            return _Context.Ticket.Where(t => t.FlightIdFk == FlightId);

        }


        //-----------------
        //Modifications

        public int GetSeatsBooked(int flightId)
        {
            return _Context.Ticket.Count(t => t.FlightIdFk == flightId && !t.Cancelled);
        }
    }
}
