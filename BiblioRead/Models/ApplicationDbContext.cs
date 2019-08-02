using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BiblioRead.Models
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {


    }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<BookRental> BookRentals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<BookRental>()
                .HasKey(br => new { br.BookId, br.RentalId});
            modelBuilder.Entity<BookRental>()
                .HasOne(br => br.Book)
                .WithMany(b => b.RentalLinks)
                .HasForeignKey(br => br.BookId);
            modelBuilder.Entity<BookRental>()
                .HasOne(br => br.Rental)
                .WithMany(r => r.BooksLink)
                .HasForeignKey(br => br.RentalId);
        }
    }
}
