using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BiblioRead.Controllers.Resources;
using BiblioRead.Models;

namespace BiblioRead.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<Book, BookResource>();
            CreateMap<Author, AuthorResource>();
        }
    }
}
