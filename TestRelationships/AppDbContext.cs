using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestRelationships.Models;

namespace TestRelationships
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Person> People { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<PersonCity> PersonCities { get; set; }
        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Person>().HasKey(p => p.Id);

            //modelBuilder.Entity<City>().HasKey(c => c.Id);

            //modelBuilder.Entity<Person>().HasOne(c => c.City).WithMany(c => c.People).HasForeignKey(c => c.CityId);


            modelBuilder.Entity<PersonCity>().HasKey(pc => new { pc.PersonId, pc.CityId });

            //modelBuilder.Entity<City>()
            //    .HasData(
            //        new City(){Id = 1, Name = "Moscow" }
            //    );

            //modelBuilder.Entity<Person>()
            //    .HasData(
            //        new Person() { Id = 1, Name = "Rus",CityId = 1 }
            //    );

            modelBuilder.Entity<Person>()
                .HasData(
                    new Person { Id = 1, Name = "Test" }
                );

            modelBuilder.Entity<City>()
                .HasData(
                    new City { Id = 1, Name = "Moscow" }
                );

            modelBuilder.Entity<PersonCity>().HasData(
                    new PersonCity { CityId = 1, PersonId = 1 }
                );

        }
    }
}
