using Domain.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Presentation.Models.ViewModel
{
    public class BookViewModel
    {

        public int Row { get; set; }

        public string Column { get; set; }

        public int FlightId { get; set; }

        public string Passport { get; set; }

        public decimal PricePaid { get; set; }

        public bool cancelled { get; set; }

    }
}
