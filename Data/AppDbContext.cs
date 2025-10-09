using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HACT.Models;

namespace HACT.Data
{
    // Κληρονομεί από IdentityDbContext για να έχει tables του Identity (Users, Roles, κλπ)
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Δικά σου tables
        public DbSet<ContactMessage> ContactMessages { get; set; }
    }
}
