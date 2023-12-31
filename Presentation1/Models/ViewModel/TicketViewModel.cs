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

        public string DepartureDate { get; set; }
        public string ArrivalDate { get; set; }
        public string CountryFrom { get; set; }

        public string CountryTo { get; set; }

        public string? image {  get; set; }
      
    }
}
