using Microsoft.EntityFrameworkCore;
using TodoList.Data;
using Microsoft.Extensions.DependencyInjection;
using Bunit;

namespace TodoList.TodoListTest;

public class Tests
{
    private Bunit.TestContext testContext = null!;

    [OneTimeSetUp]
    public void StaticSetup()
    {
        testContext = new Bunit.TestContext();

        // Create in memory test database
        testContext.Services.AddSingleton<IDbContextFactory<DatabaseContext>>(new TodoListTest.DbContextFactory());

        // Seed test database
        using (var scope = testContext.Services.CreateScope())
        {
            var service = scope.ServiceProvider.GetRequiredService<IDbContextFactory<DatabaseContext>>();
            using (var context = service.CreateDbContext())
            {
                context.Todos.Add(new TodoList.Models.Todo { Id = 1, Title = "Brush Teeth", IsDone = false });
                context.SaveChanges();
            }
        }
    }

    [OneTimeTearDown]
    public void StaticTeardown()
    {
        testContext.Dispose();
    }

    [Test]
    public void CanViewIndex()
    {
        var component = testContext.RenderComponent<TodoList.Pages.Index>();
        Assert.True(component.Markup.Contains("<h1>Welcome!</h1>"));
    }

    [Test]
    public void CanViewTodos()
    {
        using (var scope = testContext.Services.CreateScope())
        {
            var service = scope.ServiceProvider.GetRequiredService<IDbContextFactory<DatabaseContext>>();
            using (var context = service.CreateDbContext())
            {
                // verify data is in database
                var todos = context.Todos.ToList();
                Assert.AreEqual("Brush Teeth", todos.First().Title);

                // verify it rendered on the page
                var component = testContext.RenderComponent<TodoList.Pages.Todos>();
                Assert.True(component.Markup.Contains("<h1>Todos</h1>"));
                Assert.True(component.Markup.Contains(@"Brush Teeth"));
            }
        }
    }

    [Test]
    public void CanViewTodo()
    {
        using (var scope = testContext.Services.CreateScope())
        {
            var service = scope.ServiceProvider.GetRequiredService<IDbContextFactory<DatabaseContext>>();
            using (var context = service.CreateDbContext())
            {
                var component = testContext.RenderComponent<TodoList.Pages.Todo>(parameters => parameters.Add(p => p.Id, "1"));
                Assert.True(component.Markup.Contains("<h1>Todo 1</h1>"));
            }
            
        }
    }

    [Test]
    public void AllowCreateTodo()
    {
        using (var scope = testContext.Services.CreateScope())
        {
            var service = scope.ServiceProvider.GetRequiredService<IDbContextFactory<DatabaseContext>>();
            using (var context = service.CreateDbContext())
            {
                Assert.AreEqual(1, context.Todos.ToList().Count);
                var component = testContext.RenderComponent<TodoList.Pages.Todos>();
                var inputField = component.Find("input");
                inputField.Change("Buy Cheese");
                component.Find("button[type=submit].new-todo").Click();

                var todos = context.Todos.ToList();
                Assert.AreEqual(2, todos.Count);
                Assert.AreEqual("Buy Cheese", todos.Last().Title);
            }
        }
    }
}
