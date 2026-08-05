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
    }
}
