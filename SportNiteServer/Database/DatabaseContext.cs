using Microsoft.EntityFrameworkCore;
using SportNiteServer.Entities;

namespace SportNiteServer.Database;

public class DatabaseContext : DbContext
{
    public DbSet<User?> Users { get; set; }
    public DbSet<Offer> Offers { get; set; }
    public DbSet<Response> Responses { get; set; }

    public string DbPath { get; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder
            .UseMySQL("server=localhost;database=sportnite;user=root;password=12345678")
            .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
            .EnableSensitiveDataLogging();
}