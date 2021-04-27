using Cleemy.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cleemy.Data
{
    public class PurchaseContext : DbContext
    {
        public PurchaseContext(DbContextOptions<PurchaseContext> options) : base(options)
        {
        }

        public DbSet<Nature> Natures { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Amount> Amounts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Expense> Expenses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Nature>().ToTable("Nature");
            modelBuilder.Entity<Currency>().ToTable("Currency");
            modelBuilder.Entity<Amount>().ToTable("Amount");
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Expense>().ToTable("Expense");

            modelBuilder.Entity<User>()
                .HasMany(u => u.Expenses)
                .WithOne(e => e.User)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Expense>()
                .HasOne(e => e.User)
                .WithMany(u => u.Expenses)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
