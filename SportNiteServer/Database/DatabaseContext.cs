using Microsoft.EntityFrameworkCore;
using SportNiteServer.Entities;

namespace SportNiteServer.Database;

public class DatabaseContext : DbContext
{
    public DbSet<User?> Users { get; set; }
    public DbSet<Offer> Offers { get; set; }
    public DbSet<Response> Responses { get; set; }
    public DbSet<Skill> Skills { get; set; }
    public DbSet<Place> Places { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder
            .UseMySql(Environment.GetEnvironmentVariable("MYSQL_CONNECTION") ??
                      "server=localhost;database=sportnite;user=root;password=12345678",
                ServerVersion.AutoDetect(
                    Environment.GetEnvironmentVariable("MYSQL_CONNECTION") ??
                    "server=localhost;database=sportnite;user=root;password=12345678"),
                sqlOptions
                    => sqlOptions.UseNetTopologySuite())
            .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
            .EnableSensitiveDataLogging();
}