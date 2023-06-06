using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace WebApplication2
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Detail> Details { get; set; } = null!;
        public DbSet<Country> Countries { get; set; } = null!;

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.LogTo(Console.WriteLine);
            // optionsBuilder.LogTo(Console.WriteLine, new[] { RelationalEventId.CommandExecuted });
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // использование Fluent API
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Country>().HasData(
                new Country { Id = 1, Name = "ewfewf" },
                new Country { Id = 2, Name = "gfgdfg" },
                new Country { Id = 3, Name = "nbvnv" },
                new Country { Id = 4, Name = "asas" },
                new Country { Id = 5, Name = "lililil" }
            );


            modelBuilder.Entity<Detail>().HasData(
                new Detail
                {
                    Id = 21, a = 1, b = 2, c = 3, d = 4, e = 5, f = "object0 f", k = "object0 k", m = "object0 m", CountryId = 1
                },
                new Detail
                {
                    Id = 22, a = 21, b = 22, c = 23, d = 24, e = 25, f = "object2 f", k = "object2 k", m = "object2 m", CountryId = 2
                },
                new Detail
                {
                    Id = 23, a = 11, b = 12, c = 13, d = 14, e = 15, f = "object1 f", k = "object1 k", m = "object1 m", CountryId = 3
                },
                new Detail
                {
                    Id = 24, a = 31, b = 32, c = 33, d = 34, e = 35, f = "object3 f", k = "object3 k", m = "object3 m", CountryId = 4
                },
                new Detail
                {
                    Id = 25, a = 41, b = 42, c = 43, d = 44, e = 45, f = "object4 f", k = "object4 k", m = "object4 m", CountryId = 5
                },
                new Detail
                {
                    Id = 26, a = 51, b = 52, c = 53, d = 54, e = 55, f = "object5 f", k = "object5 k", m = "object5 m", CountryId = 1
                },
                new Detail
                {
                    Id = 27, a = 61, b = 62, c = 63, d = 64, e = 65, f = "object6 f", k = "object6 k", m = "object6 m", CountryId = 2
                },
                new Detail
                {
                    Id = 28, a = 71, b = 72, c = 73, d = 74, e = 75, f = "object7 f", k = "object7 k", m = "object7 m", CountryId = 3
                },
                new Detail
                {
                    Id = 29, a = 81, b = 82, c = 83, d = 84, e = 85, f = "object8 f", k = "object8 k", m = "object8 m", CountryId = 4
                }
            );
        }
    }
}