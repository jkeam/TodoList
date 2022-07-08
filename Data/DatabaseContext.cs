namespace TodoList.Data;
using TodoList.Models;

using Microsoft.EntityFrameworkCore;

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