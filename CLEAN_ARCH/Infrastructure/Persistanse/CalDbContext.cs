using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistanse
{
    public class CalDbContext : IdentityDbContext
    {
        public CalDbContext(DbContextOptions<CalDbContext> options) 
            : base(options) { }

        public DbSet<CalEvent> CalEvents { get; set; } = null!;

        public DbSet<CalEventCategory> CalEventCategories { get; set; } = null!;

        public DbSet<RequestHeader> RequestsHeaders { get; set; } = null!;
    }
}
