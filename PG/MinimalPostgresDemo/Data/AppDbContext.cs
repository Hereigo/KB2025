using Microsoft.EntityFrameworkCore;

namespace MinimalPostgresDemo.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Todo> Todos => Set<Todo>();
    }

    public class Todo
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public bool IsComplete { get; set; }
    }
}
