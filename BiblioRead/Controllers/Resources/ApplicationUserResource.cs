using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace BiblioRead.Controllers.Resources
{
    public class ApplicationUserResource
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string FullName { get; set; }

        public string Role { get; set; }

        public IList<RentalResource> Rentals { get; set; }

        public ApplicationUserResource()
        {
            Rentals = new Collection<RentalResource>();
        }
    }
}
