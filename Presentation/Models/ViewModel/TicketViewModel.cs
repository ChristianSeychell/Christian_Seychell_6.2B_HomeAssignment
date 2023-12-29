using Domain.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Presentation.Models.ViewModel
{
    public class TicketViewModel
    {
        public int Id { get; set; }

        public string Seat { get; set; }

        public int FlightId { get; set; }

        public string Passport { get; set; }

        public decimal PricePaid { get; set; }

        public Boolean Cancelled { get; set; }
    }
}
