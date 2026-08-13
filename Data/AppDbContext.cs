using Microsoft.EntityFrameworkCore;
using TaskMenagementAPI.Models;

namespace TaskMenagementAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; } = null!;

        public DbSet<Project> Projects { get; set; } = null!;

        public DbSet<ProjectTask> Tasks { get; set; } = null!;

        public DbSet<ProjectMember> Members { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Project>()
                .HasMany(p => p.Tasks)
                .WithOne(t => t.Project)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProjectMember>()
                .HasOne(pm => pm.Project)
                .WithMany(p => p.Members)
                .HasForeignKey(pm => pm.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProjectMember>()
                .HasOne(pm => pm.User)
                .WithMany(u => u.ProjectMemberships)
                .HasForeignKey(pm => pm.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ProjectMember>()
                .HasIndex(pm => new { pm.UserId, pm.ProjectId })
                .IsUnique();

            modelBuilder.Entity<ProjectTask>()
                .HasOne(t => t.Assignee)
                .WithMany()
                .HasForeignKey(t => t.AssigneedId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
