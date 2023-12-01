using Microsoft.EntityFrameworkCore;

namespace task_management_system.Models;

public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
	{

	}
        public DbSet<TaskModel> Tasks { get; set; }

	 protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Add configurations for your existing data...
    }
}
