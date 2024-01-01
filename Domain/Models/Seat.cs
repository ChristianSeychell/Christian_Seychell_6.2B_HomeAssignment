using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Seat
    {
        public int Row { get; set; }
        public int Column { get; set; }

        public string Id { get { return Row + "," + Column; } }
    }
}
