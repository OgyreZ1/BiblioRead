using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BiblioRead.Controllers.Resources;
using BiblioRead.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BiblioRead.Controllers
{
    [Route("api/authors")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AuthorsController(ApplicationDbContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<AuthorResource>> GetAuthors()
        {
            var authors = await _context.Authors.Include(b => b.Books).ToListAsync();

            return _mapper.Map<List<Author>, List<AuthorResource>>(authors);
        }

        [HttpGet("{id}")]
        public async Task<AuthorResource> GetAuthor(int id)
        {
            var author = await _context.Authors.Include(b => b.Books).SingleOrDefaultAsync(b => b.Id == id);

            return _mapper.Map<Author, AuthorResource>(author);
        }


    }
}