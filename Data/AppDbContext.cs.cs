using Catering.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Catering.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<ItemCategory> ItemCategories { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingItem> BookingItems { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Invoice> Invoices { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ItemCategory>()
            .HasKey(c => c.CategoryId);

            // Messages: prevent cascade‑delete cycles between users
            builder.Entity<Message>()
                .HasOne(m => m.FromUser)
                .WithMany()                // or .WithMany(u => u.SentMessages)
                .HasForeignKey(m => m.FromUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
                .HasOne(m => m.ToUser)
                .WithMany()                // or .WithMany(u => u.ReceivedMessages)
                .HasForeignKey(m => m.ToUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Favorites: each customer → many favorites, each favorite → one caterer
            builder.Entity<Favorite>()
                .HasOne(f => f.Customer)
                .WithMany()                // or .WithMany(u => u.Favorites)
                .HasForeignKey(f => f.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Favorite>()
                .HasOne(f => f.Caterer)
                .WithMany()                // or .WithMany(u => u.FavoritedBy)
                .HasForeignKey(f => f.CatererId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Booking>()
                .HasOne(b => b.Customer)
                .WithMany() // or .WithMany(u => u.CustomerBookings) 
                .HasForeignKey(b => b.CustomerId)
                .OnDelete(DeleteBehavior.Restrict); 
                

            builder.Entity<Booking>()
                .HasOne(b => b.Caterer)
                .WithMany() // or .WithMany(u => u.CatererBookings)
                .HasForeignKey(b => b.CatererId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Booking>()
                .Property(b => b.InvoiceAmount)
                .HasPrecision(18, 2);

            builder.Entity<BookingItem>()
                .Property(bi => bi.Price)
                .HasPrecision(18, 2);

            builder.Entity<Invoice>()
                .Property(i => i.Amount)
                .HasPrecision(18, 2);

            builder.Entity<Item>()
                .Property(i => i.Price)
                .HasPrecision(18, 2);

        }
    }
}

