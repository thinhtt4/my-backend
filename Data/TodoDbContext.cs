using Microsoft.EntityFrameworkCore;
using TodoBackend.Models;


namespace MyFirstBackend.Data
{
    public class TodoDbContext : DbContext
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options)
            : base(options) { }

        public DbSet<TodoItem> TodoItems => Set<TodoItem>();
    }
}