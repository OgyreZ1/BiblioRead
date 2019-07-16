using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BiblioRead.Models;

namespace BiblioRead.Controllers.Resources
{
    public class AuthorResource
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public ICollection<BookResource> Books { get; set; }

        public AuthorResource()
        {
            Books = new Collection<BookResource>();
        }

    }
}
