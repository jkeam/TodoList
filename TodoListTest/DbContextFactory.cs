using Microsoft.EntityFrameworkCore;
using TodoList.Data;

namespace TodoList.TodoListTest
{
    public class DbContextFactory: IDbContextFactory<DatabaseContext>
    {
        public DatabaseContext CreateDbContext()
        {
            DbContextOptions<DatabaseContext> options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "TodoAppTest").Options;
            return new DatabaseContext(options);
        }

        public Task<DatabaseContext> CreateDbContextAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(CreateDbContext());
        }
    }
}