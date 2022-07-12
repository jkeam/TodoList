using TodoList.Data;
using Microsoft.EntityFrameworkCore;

string GetConfigValue(IConfigurationSection section, string keyName, string defaultValue)
{
    var environmentValue = Environment.GetEnvironmentVariable(keyName);
    if (!String.IsNullOrEmpty(environmentValue))
    {
        return environmentValue;
    }

    return section.GetValue<string>(keyName, defaultValue);
}

var builder = WebApplication.CreateBuilder(args);
var section = builder.Configuration.GetSection("AppConfig");

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddDbContextFactory<DatabaseContext>(options =>
  options.UseSqlServer(GetConfigValue(section, "DB_URL", "")));
builder.Services.AddSingleton<Config>(
    new Config(GetConfigValue(section, "APP_NAME", "TodoList")));

var app = builder.Build();

// Migration
var migrate = GetConfigValue(section, "MIGRATION", "false");
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

// app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
