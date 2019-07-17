using Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace Web.Data
{
    public class BookingContext : IdentityDbContext<IdentityUser>
    {
        
        static object locker = new object();

        public BookingContext(DbContextOptions<BookingContext> options) : base(options)
        {
            
            lock (locker)
            {
                if (Database.GetPendingMigrations().Any())
                {
                    Database.Migrate();
                }
            }
        }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<Holidays> Holidays { get; set; }
       
         protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}