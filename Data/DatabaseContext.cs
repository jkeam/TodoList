using TodoList.Models;

using Microsoft.EntityFrameworkCore;

namespace TodoList.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public virtual DbSet<Todo> Todos { get; set; } = null!;
    }
}