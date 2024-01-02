using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Models.ViewModel
{
    public class FlightListViewModel
    {
        public int Id { get; set; }

        public int Seats { get; set; }

        public DateTime DepartureDate { get; set; }

        public DateTime Arrivaldate { get; set; }

        public string CountryFrom { get; set; }

        public string CountryTo { get; set; }

        public decimal price { get; set; }

        
    }
}
