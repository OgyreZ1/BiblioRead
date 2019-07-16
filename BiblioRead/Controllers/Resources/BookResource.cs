using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BiblioRead.Controllers.Resources
{
    public class BookResource {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        [StringLength(255)]
        public string AuthorName { get; set; }

        public int AuthorId { get; set; }

        

    }
}
