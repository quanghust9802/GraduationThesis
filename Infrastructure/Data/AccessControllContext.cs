using Domain.Entities;
using Microsoft.EntityFrameworkCore;


//configure fluent api
namespace Infrastructure.Data
{
    public class AccessControllContext : DbContext
    {
        public AccessControllContext(DbContextOptions<AccessControllContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<AccessRequest> AccessRequests { get; set; }

        public DbSet<AccessLogs> AccessLogs { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Notification>()
                .HasOne(u => u.User)
                .WithMany(n => n.Notifications)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.NoAction);







        }
    }
}