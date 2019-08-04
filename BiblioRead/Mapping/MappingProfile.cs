using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BiblioRead.Controllers.Resources;
using BiblioRead.Models;
using Microsoft.AspNetCore.Identity;

namespace BiblioRead.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<Book, BookResource>();
            CreateMap<Author, AuthorResource>();
            CreateMap<ApplicationUser, ApplicationUserResource>();
            CreateMap<IdentityUser, ApplicationUserResource>();
            CreateMap<Rental, RentalResource>()
            .ForMember(rr => rr.UserId, opt => opt.MapFrom(r => r.User.Id));
        }
    }
}
