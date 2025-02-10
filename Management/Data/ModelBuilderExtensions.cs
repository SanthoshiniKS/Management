using Management.Models;
using Microsoft.EntityFrameworkCore;

namespace Management.Data
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>().HasData(
                new City {Id=1, Name = "Madurai" },
                new City {Id=2, Name = "Chennai" },
                new City { Id=3,Name = "Bangalore" },
                new City { Id=4,Name = "Kolkata" },
                new City { Id=5,Name = "West Bengal" }, 
                new City { Id=6,Name = "Delhi" }, 
                new City { Id=7,Name = "Agra" },
                new City { Id=8,Name = "Visakapattinam" }
                );
        }
    }
}
