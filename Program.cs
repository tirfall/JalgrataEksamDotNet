using JalgrataEksamDotNet.Data;
using JalgrataEksamDotNet.Models;
using JalgrataEksamDotNet.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// MVC + API.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();

// SQLite database connection.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=exams.db"));

var app = builder.Build();

// Ensure the database exists and seed initial data from XML if empty.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
    SeedData.EnsureSeeded(db, Path.Combine(app.Environment.ContentRootPath, "XML", "eksamid.xml"));
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Exams}/{action=Index}/{id?}");

app.Run();