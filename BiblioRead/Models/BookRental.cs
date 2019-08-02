using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiblioRead.Models
{
    public class BookRental
    {
        public int BookId { get; set; }

        public Book Book { get; set; }

        public int RentalId { get; set; }

        public Rental Rental { get; set; }
    }
}
