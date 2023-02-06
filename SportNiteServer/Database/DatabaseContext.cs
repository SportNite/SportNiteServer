using Microsoft.EntityFrameworkCore;
using SportNiteServer.Entities;

namespace SportNiteServer.Database;

public class DatabaseContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Offer> Offers => Set<Offer>();
    public DbSet<Response> Responses => Set<Response>();
    public DbSet<Skill> Skills => Set<Skill>();
    public DbSet<Place> Places => Set<Place>();
    public DbSet<Notification> Notifications => Set<Notification>();

    // Connect to database using connection string from environment variable
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