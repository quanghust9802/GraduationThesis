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
            modelBuilder.Entity<User>()
                .HasOne(u => u.UserRole)
                .WithMany(ur => ur.Users)
                .HasForeignKey(u => u.UserRoleId)
                .OnDelete(DeleteBehavior.NoAction);

            //  User - Notification (1-n)
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            //  User - AccessRequest (RequestedAccessRequests) (1-n)
            modelBuilder.Entity<AccessRequest>()
                .HasOne(ar => ar.RequestUser)
                .WithMany(u => u.RequestedAccessRequests)
                .HasForeignKey(ar => ar.UserRequestId)
                .OnDelete(DeleteBehavior.NoAction);

            //  User - AccessRequest (ApprovedAccessRequests) (1-n)
            modelBuilder.Entity<AccessRequest>()
                .HasOne(ar => ar.ApproveUser)
                .WithMany(u => u.ApprovedAccessRequests)
                .HasForeignKey(ar => ar.UserApprovalid)
                .OnDelete(DeleteBehavior.NoAction);

            //  User - AccessLogs (1-n)
            modelBuilder.Entity<AccessLogs>()
                .HasOne(al => al.User)
                .WithMany(u => u.AccessLogs)
                .HasForeignKey(al => al.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            //  AccessRequest - AccessLogs (1-n)
            modelBuilder.Entity<AccessLogs>()
                .HasOne(al => al.AccessRequest)
                .WithMany(ar => ar.AccessLogs)
                .HasForeignKey(al => al.AccessRequestId)
                .OnDelete(DeleteBehavior.NoAction);







        }
    }
}