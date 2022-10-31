using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Playground;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SportNiteServer.Data;
using SportNiteServer.Database;
using SportNiteServer.Services;

FirebaseApp.Create(new AppOptions
{
    Credential = GoogleCredential.GetApplicationDefault(),
});

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<DatabaseContext>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<AuthService>();
builder.Services.AddTransient<OfferService>();

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
    .RegisterDbContext<DatabaseContext>()
    .RegisterService<AuthService>()
    .RegisterService<OfferService>()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();
app.MapGraphQL();
app.MapGraphQLVoyager();
app.UsePlayground(new PlaygroundOptions { QueryPath = "/graphql", Path = "/playground" });

app.Run();