using CaloriePlannerApp.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaloriePlannerApp.Data
{
    public class DataContext : DbContext
    {
        public DataContext()
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<FoodItem> Foods { get; set; }
        public DbSet<FoodUser> FoodUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=maui.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly, (Type type) =>
            {
                return type.Namespace.Contains("Configurations");
            });
        }
    }
}
