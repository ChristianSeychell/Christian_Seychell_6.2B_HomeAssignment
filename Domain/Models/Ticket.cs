using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Ticket
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int Row { get; set; }

        public string Column { get; set; }

        [ForeignKey("Flight")]
        public int FlightIdFk { get; set;}

        public virtual Flight Flight { get; set; }

        public string Passport { get; set; }

        public decimal PricePaid { get; set; }

        public string? Image { get; set; }

        public Boolean Cancelled { get; set; }

    }   
}
