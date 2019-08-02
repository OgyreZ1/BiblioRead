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

        public IEnumerable<int> BookIds { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime EndingDate { get; set; }

        public bool IsCompleted { get; set; }

        public RentalResource() {
            BookIds = new Collection<int>();
        }
    }
}
