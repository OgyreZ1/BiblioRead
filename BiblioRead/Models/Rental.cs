﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace BiblioRead.Models
{
    public class Rental
    {

        public int Id { get; set; }

        public ApplicationUser User { get; set; }

        public IEnumerable<BookRental> BooksLink { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DeadlineDate { get; set; }

        public DateTime? DateFinished { get; set; }

        public bool IsFinished { get; set; }

        public Rental() {
            BooksLink = new Collection<BookRental>();
        }
    }
}
