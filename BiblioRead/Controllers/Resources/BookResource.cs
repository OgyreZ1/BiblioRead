﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiblioRead.Controllers.Resources
{
    public class BookResource
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int Year { get; set; }

        public string AuthorName { get; set; }

    }
}