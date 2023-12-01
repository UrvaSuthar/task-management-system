// IdentityDbContext.cs
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using task_management_system;

public class IdentityDbContext : IdentityDbContext<ApplicationUser>
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
    {
    }
}
