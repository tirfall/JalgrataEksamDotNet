using JalgrataEksamDotNet.Models;
using Microsoft.EntityFrameworkCore;

namespace JalgrataEksamDotNet.Data;

// DB context for the exams table.
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Exam> Exams => Set<Exam>();
}
