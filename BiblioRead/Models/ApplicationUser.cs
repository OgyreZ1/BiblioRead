using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace BiblioRead.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Column(TypeName = "nvarchar(255)")]
        public string FullName { get; set; }

        public IEnumerable<Rental> Rentals { get; set; }

        public ApplicationUser() {
            Rentals = new Collection<Rental>();
        }
    }
}
