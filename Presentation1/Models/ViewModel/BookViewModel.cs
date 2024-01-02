using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Presentation.Models.ViewModel
{
    public class BookViewModel
    {
        [Remote(action: "CheckSeatAailable", controller: "Tickets", AdditionalFields = "FlightId,Row,Column")]
        public int Row { get; set; }

        [Remote(action: "CheckSeatAailable", controller: "Tickets", AdditionalFields = "FlightId,Row,Column")]
        public string Column { get; set; }

        public int seats {  get; set; }
        public int FlightId { get; set; }

        public int Maxrows {  get; set; }  

        public int Maxcols { get; set; }

        public string Passport { get; set; }

        public decimal PricePaid { get; set; }

        public bool cancelled { get; set; }

        public IFormFile imageFile { get; set; }

    }
}
