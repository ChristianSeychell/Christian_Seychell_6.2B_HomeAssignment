using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Flight
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int Rows { get; set; }

        public int Columns { get; set; }

        public DateTime DepartureDate { get; set; }

        public DateTime Arrivaldate { get; set; }

        public string CountryFrom { get; set; }

        public string CountryTo { get; set; }

        public decimal WholesalePrice { get; set; }
        public decimal CommissionRate { get; set; }

     }
}
