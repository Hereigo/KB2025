using Microsoft.EntityFrameworkCore;
using MinimalPostgresDemo.Data;

var builder = WebApplication.CreateBuilder(args);

// PostgreSQL connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

// Seed data if Todos table is empty
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate(); // applies migrations

    if (!db.Todos.Any())
    {
        db.Todos.AddRange(
            new Todo { Title = "First task", IsComplete = false },
            new Todo { Title = "Second task", IsComplete = true }
        );
        db.SaveChanges();
    }
}

// Minimal API endpoints
app.MapGet("/todos", async (AppDbContext db) => await db.Todos.ToListAsync());

app.MapPost("/todos", async (AppDbContext db, Todo todo) =>
{
    db.Todos.Add(todo);
    await db.SaveChangesAsync();
    return Results.Created($"/todos/{todo.Id}", todo);
});

app.Run();
