using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Execution;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SportNiteServer.Data;
using SportNiteServer.Database;
using SportNiteServer.Entities;
using SportNiteServer.Exceptions;
using SportNiteServer.Services;

namespace SportNiteServer.Tests;

public class QueryIntegrationTests
{
    private IRequestExecutor _executor;
    private IServiceProvider _serviceProvider;

    private const string TestUserFirebaseId = "aaaaaaaaaaaaaaaaaaaaaaaaaaaa";
    private const string TestUserPhone = "48123456789";

    [SetUp]
    public async Task Setup()
    {
        var serviceCollection = new ServiceCollection()
            .AddDbContext<DatabaseContext>(ServiceLifetime.Transient)
            .AddTransient<AuthService>()
            .AddTransient<OfferService>()
            .AddTransient<ResponseService>()
            .AddTransient<WeatherService>()
            .AddSingleton<PlaceService>()
            .AddSingleton<UserService>();

        var schema = await serviceCollection
            .AddGraphQLServer()
            .AddType<PointSortType>()
            .AddSpatialTypes()
            .AddAuthorization()
            .AddSorting()
            .AddFiltering()
            .AddSpatialFiltering()
            .AddSpatialProjections()
            .RegisterDbContext<DatabaseContext>()
            .RegisterService<AuthService>()
            .RegisterService<OfferService>()
            .RegisterService<ResponseService>()
            .RegisterService<WeatherService>()
            .RegisterService<PlaceService>()
            .RegisterService<UserService>()
            .AddQueryType<Query>()
            .AddMutationType<Mutation>()
            .AddErrorFilter<GraphQlErrorFilter>()
            .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true)
            .BuildSchemaAsync();

        _serviceProvider = serviceCollection.BuildServiceProvider();
        _executor = schema.MakeExecutable();
    }

    private async Task<string> Query(string query)
    {
        var claims = new List<Claim>
        {
            new("user_id", TestUserFirebaseId),
            new("phone_number", TestUserPhone)
        };
        var claimsPrincipal = new ClaimsPrincipal();
        claimsPrincipal.AddIdentity(new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme));
        var request = QueryRequestBuilder.New()
            .SetQuery(query)
            .SetServices(_serviceProvider)
            .AddProperty(nameof(ClaimsPrincipal), claimsPrincipal)
            .Create();
        var result = await _executor.ExecuteAsync(request);
        var output = await result.ToJsonAsync();
        return output;
    }

    [Test]
    public async Task Version()
    {
        StringAssert.Contains("1.0.0", await Query("{version}"));
    }

    [Test]
    public async Task Me()
    {
        StringAssert.Contains(TestUserPhone, await Query("query {  me {    firebaseUserId    phone  }}"));
    }

    [Test]
    public async Task Offers()
    {
        await Query(
            "mutation {  createOffer(    input: {      dateTime: \"2022-12-12\"      sport: TENNIS      street: \"Mickiewicza\"      city: \"Krakow\"      placeId: 0    }  ) {    offerId    sport    dateTime  }}");

        var result = await Query(@"query {
              offers(last: 50) {
                nodes {
                  offerId
                  sport
                  street
                  city
                  dateTime
                  user { userId } description isAvailable responses { responseId } place { id } weather { windSpeed } 
                }
              }
            }");
        StringAssert.Contains("Krakow", result);
        StringAssert.Contains("Mickiewicza", result);
        StringAssert.Contains("TENNIS", result);
        StringAssert.Contains("2022-12", result);
    }

    [Test]
    public async Task Users()
    {
        var result = await Query(@"query {
              users {
                nodes {
                  firebaseUserId
                  phone
                }
              }
            }");
        StringAssert.Contains(TestUserFirebaseId, result);
        StringAssert.Contains(TestUserPhone, result);
    }

    [Test]
    public async Task MyOffers()
    {
        await Query(
            "mutation {  createOffer(    input: {      dateTime: \"2022-12-12\"      sport: TENNIS      street: \"Mickiewicza\"      city: \"Krakow\"      placeId: 0    }  ) {    offerId    sport    dateTime  }}");
        var result = await Query(@"query {
              myOffers(last: 50) {
                nodes {
                  offerId
                  sport
                  street
                  city
                  dateTime
                }
              }
            }");
        StringAssert.Contains("Krakow", result);
        StringAssert.Contains("Mickiewicza", result);
        StringAssert.Contains("TENNIS", result);
        StringAssert.Contains("2022-12", result);
    }

    [Test]
    public async Task MyResponses()
    {
        await Query(
            "mutation {  createResponse(    input: {      offerId: \"08daeb2d-60b9-4241-8d96-095d3eee6acc\"      description: \"Test description\"    }  ) {    offerId    description    responseId  }}");

        var result = await Query(@"
            query {
              myResponses(last: 50) {
                nodes {
                  offerId
                  description
                  responseId
                  offer {
                   offerId
                  }
                  user {
                   userId
                  }
                }
              }
            }
            ");
        StringAssert.Contains("Test description", result);
    }

    [Test]
    public async Task IncomingOffers()
    {
        var result = await Query(@"
           query {
              incomingOffers {
                offerId
                description
              }
            }
            ");
        StringAssert.Contains("data", result);
    }

    [Test]
    public async Task Places()
    {
        var result = await Query(@"
           query {
              places {
                id
                name
              }
            }
            ");
        StringAssert.Contains("Hotel", result);
    }

    [Test]
    public async Task Forecast()
    {
        var result =
            await Query(
                "query {  forecast(startDay: \"2022-12-12\", latitude: 1, longitude: 1) {    windSpeed    precipitation    temperature    dateTime  }}");
        for (var i = 10; i < 20; i++) StringAssert.Contains($"{i}:00:00.000", result);
    }
}