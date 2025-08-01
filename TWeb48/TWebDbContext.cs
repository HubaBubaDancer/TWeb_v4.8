using System.Data.Entity;
using TWeb48.Models;

namespace TWeb48
{
    public class TWebDbContext : DbContext
    {
        public TWebDbContext() : base("name=TWebDbContext")
        {
        }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Rent> Rents { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasRequired(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasRequired(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            base.OnModelCreating(modelBuilder);
        }
        
    }
}