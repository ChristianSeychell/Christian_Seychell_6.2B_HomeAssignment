using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ITicketRepository
    {


        void Book(Ticket ticket);


        void cancel(Ticket ticket);

        IQueryable<Ticket> GetTickets();

        int GetSeatsBooked(int flightId);

        IQueryable<Ticket> GetTicketInfo(int ticketId);


        IQueryable<Ticket> GetFlightTickets(int FlightId);
    }
}
