using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using TodoList.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddDbContextFactory<DatabaseContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("DB_URL")));

var app = builder.Build();

// Migration
var migrate = Environment.GetEnvironmentVariable("MIGRATION");
if (migrate != null && migrate.ToLower() == "true") {
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        Console.WriteLine("Database migrating...");
        db.Database.Migrate();
        Console.WriteLine("Database migrated");
    }
    return;
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
