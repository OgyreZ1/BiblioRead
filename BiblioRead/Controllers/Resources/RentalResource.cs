using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace BiblioRead.Controllers.Resources
{
    public class RentalResource
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public IEnumerable<BookResource> Books { get; set; }

        public IEnumerable<int> BookIds { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DeadlineDate { get; set; }

        public DateTime? DateFinished { get; set; }

        public bool IsFinished { get; set; }

        public RentalResource() {
            Books = new Collection<BookResource>();
            BookIds = new Collection<int>();
        }
    }
}
