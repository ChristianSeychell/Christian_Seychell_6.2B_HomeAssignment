using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Models.ViewModel
{
    public class FlightListViewModel
    {
        public int Id { get; set; }

        public int Rows { get; set; }

        public string Columns { get; set; }

        public DateTime DepartureDate { get; set; }

        public DateTime Arrivaldate { get; set; }

        public string CountryFrom { get; set; }

        public string CountryTo { get; set; }

        public float price { get; set; }
    }
}
