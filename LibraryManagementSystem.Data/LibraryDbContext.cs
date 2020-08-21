using LibraryManagementSystem.Data.Models;
using System.Data.Entity;

namespace LibraryManagementSystem.Data
{

    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext() : base("LibraryConnection")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<LibraryDbContext, Migrations.Configuration>());
        } 

        public virtual DbSet<Book> Books { get; set; } 
        public virtual DbSet<Checkout> Checkouts { get; set; }
        public virtual DbSet<CheckoutHistory> CheckoutHistories { get; set; }  
        public virtual DbSet<LibraryCard> LibraryCards { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Status> Statuses { get; set; }
        public virtual DbSet<LibraryAsset> LibraryAssets { get; set; }
        public virtual DbSet<Hold> Holds { get; set; }
        public virtual DbSet<Overdue> Overdues { get; set; }
        public virtual DbSet<Charges> Charges { get; set; }
    }
}
