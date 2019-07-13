using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BiblioRead.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        public int Year { get; set; }


        public Author Author { get; set; }

        public int AuthorId { get; set; }

        
    }
}
