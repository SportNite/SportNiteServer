using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Playground;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SportNiteServer.Data;
using SportNiteServer.Database;
using SportNiteServer.Services;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseSentry(o =>
{
    o.Dsn = "https://13bfcb853a83456492fdfd3c4a889596@o337011.ingest.sentry.io/4504181875933184";
    // When configuring for the first time, to see what the SDK is doing:
    o.Debug = true;
    // Set TracesSampleRate to 1.0 to capture 100% of transactions for performance monitoring.
    // We recommend adjusting this value in production.
    o.TracesSampleRate = 1.0;
});


// Add services to the container.

builder.Services.AddDbContext<DatabaseContext>(ServiceLifetime.Transient);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<AuthService>();
builder.Services.AddTransient<OfferService>();
builder.Services.AddTransient<ResponseService>();
builder.Services.AddTransient<WeatherService>();
builder.Services.AddSingleton<PlaceService>();
builder.Services.AddSingleton<UserService>();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://securetoken.google.com/sportnite-7b070";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = "https://securetoken.google.com/sportnite-7b070",
            ValidateAudience = true,
            ValidAudience = "sportnite-7b070",
            ValidateLifetime = true
        };
    });
builder.Services
    .AddGraphQLServer()
    .AddAuthorization()
    .AddSorting()
    .AddFiltering()
    .RegisterDbContext<DatabaseContext>()
    .RegisterService<AuthService>()
    .RegisterService<OfferService>()
    .RegisterService<ResponseService>()
    .RegisterService<WeatherService>()
    .RegisterService<PlaceService>()
    .RegisterService<UserService>()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseSentryTracing();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();
app.MapGraphQL();
app.MapGraphQLVoyager();
app.UsePlayground(new PlaygroundOptions { QueryPath = "/graphql", Path = "/playground" });

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<DatabaseContext>();
    if (context.Database.GetPendingMigrations().Any())
        context.Database.Migrate();
}

app.Run();