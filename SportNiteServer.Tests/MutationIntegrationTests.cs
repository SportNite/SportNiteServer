using System;
using System.Collections.Generic;
using System.Security.Claims;
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

public class MutationIntegrationTests
{
    private IRequestExecutor _executor = null!;
    private IServiceProvider _serviceProvider = null!;

    private const string TestUserFirebaseId = "aaaaaaaaaaaaaaaaaaaaaaaaaaaa";
    private const string TestUserPhone = "48123456789";

    [SetUp]
    public async Task Setup()
    {
        // Setup DI
        var serviceCollection = new ServiceCollection()
            .AddDbContext<DatabaseContext>(ServiceLifetime.Transient)
            .AddTransient<AuthService>()
            .AddTransient<OfferService>()
            .AddTransient<ResponseService>()
            .AddTransient<WeatherService>()
            .AddSingleton<PlaceService>()
            .AddSingleton<UserService>();

        // Setup GraphQL execution engine
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

    // Perform a GraphQL query and return the result as JSON string
    private async Task<string> Query(string query)
    {
        // Inject fake user into GraphQL context
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
    public async Task CreateOffer()
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
                }
              }
            }");
        StringAssert.Contains("Krakow", result);
        StringAssert.Contains("Mickiewicza", result);
        StringAssert.Contains("TENNIS", result);
        StringAssert.Contains("2022-12", result);
    }

    [Test]
    public async Task CreateResponse()
    {
        await Query(
            "mutation {  createOffer(    input: {    offerId: \"08daeb2d-60b9-4241-8d96-095d3eee6acc\"     dateTime: \"2022-12-12\"      sport: TENNIS      street: \"Mickiewicza\"      city: \"Krakow\"      placeId: 0    }  ) {    offerId    sport    dateTime  }}");
        await Query(
            "mutation {  createResponse(    input: {      offerId: \"08daeb2d-60b9-4241-8d96-095d3eee6acc\"      description: \"Test description\"    }  ) {    offerId    description    responseId  }}");

        var result = await Query(@"
            query {
              myResponses(last: 50) {
                nodes {
                  offerId
                  description
                  responseId
                }
              }
            }
            ");
        StringAssert.Contains("Test description", result);
    }

    [Test]
    public async Task Me()
    {
        await Query("mutation {  updateUser(payload: { bio: \"Test bio\" }) {    firebaseUserId  }}");
        StringAssert.Contains("Test bio", await Query("query {  me {    firebaseUserId    phone bio  }}"));
    }

    [Test]
    public async Task DeleteOffer()
    {
        await Query(
            "mutation {  createOffer(    input: {      offerId: \"72d2594c-a955-47e0-aca1-024715d719e4\"      dateTime: \"2022-12-01\"      sport: TENNIS      street: \"Mickiewicza\"      city: \"Krakow\"      placeId: 0    }  ) {    offerId    sport    dateTime  }}");

        var result = await Query(@"query {
              offers(last: 50) {
                nodes {
                  offerId
                  sport
                  street
                  city
                  dateTime
                }
              }
            }");
        StringAssert.Contains("72d2594c-a955-47e0-aca1-024715d719e4", result);
        await Query("mutation {  deleteOffer(id: \"72d2594c-a955-47e0-aca1-024715d719e4\") {    offerId  }}");

        var result2 = await Query(@"query {
              offers(last: 50) {
                nodes {
                  offerId
                  sport
                  street
                  city
                  dateTime
                }
              }
            }");
        StringAssert.DoesNotContain("72d2594c-a955-47e0-aca1-024715d719e4", result2);
    }


    [Test]
    public async Task DeleteResponse()
    {
        await Query(
            "mutation {  createOffer(    input: {      offerId: \"72d2594c-a955-47e0-aca1-024715d719e5\"      dateTime: \"2022-12-01\"      sport: TENNIS      street: \"Mickiewicza\"      city: \"Krakow\"      placeId: 0    }  ) {    offerId    sport    dateTime  }}");

        await Query(
            "mutation {  createResponse(    input: {      responseId: \"72d2594c-a955-47e0-aca1-024715d719e4\"      offerId: \"72d2594c-a955-47e0-aca1-024715d719e5\"      description: \"Test description\"    }  ) {    offerId    description    responseId  }}");

        var result = await Query(@"
            query {
              myResponses(last: 50) {
                nodes {
                  offerId
                  responseId
                  description
                  responseId
                }
              }
            }
            ");
        StringAssert.Contains("72d2594c-a955-47e0-aca1-024715d719e4", result);

        await Query("mutation {  deleteResponse(id: \"72d2594c-a955-47e0-aca1-024715d719e4\") {    responseId  }}");

        var result2 = await Query(@"
            query {
              myResponses(last: 50) {
                nodes {
                  offerId
                  responseId
                  description
                  responseId
                }
              }
            }
            ");
        StringAssert.DoesNotContain("72d2594c-a955-47e0-aca1-024715d719e4", result2);
    }

    [Test]
    public async Task SetSkill()
    {
        await Query(
            "mutation {  setSkill(input: { sport: TENNIS, nrtp: 5, level: 7, years: 3 }) {    sport    skillId    years    nrtp  }}");
        var result = await Query(@"
            query {
              me {
                firebaseUserId
                skills {
                  sport
                  skillId
                  years
                  nrtp
                  level 
                  weight
                  height
                }
              }
            }
            ");
        StringAssert.Contains("TENNIS", result);
        StringAssert.Contains("3", result);
        StringAssert.Contains("5", result);
        StringAssert.Contains("7", result);
    }

    [Test]
    public async Task DeleteSkill()
    {
        await Query(
            "mutation {  setSkill(input: { sport: TENNIS, nrtp: 5, level: 7, years: 3 }) {    sport    skillId    years    nrtp  }}");
        var result = await Query(@"
            query {
              me {
                firebaseUserId
                skills {
                  sport
                  skillId
                  years
                  nrtp
                  level
                }
              }
            }
            ");
        StringAssert.Contains("TENNIS", result);
        StringAssert.Contains("3", result);
        StringAssert.Contains("5", result);
        StringAssert.Contains("7", result);

        await Query(@"
        mutation {
          deleteSkill(sportType: TENNIS) {
            sport
          }
        }
        ");

        var result2 = await Query(@"
            query {
              me {
                skills {
                  sport
                  skillId
                  years
                  nrtp
                  level
                }
              }
            }
            ");
        StringAssert.DoesNotContain("TENNIS", result2);
        StringAssert.DoesNotContain("3", result2);
        StringAssert.DoesNotContain("5", result2);
        StringAssert.DoesNotContain("7", result2);
    }

    [Test]
    public async Task CreatePlace()
    {
        await Query(
            "mutation {  createPlace(    input: {      id: 8      sport: \"swimming\"      latitude: 1      longitude: 2      name: \"Plywalnia MUSZELKA\"    }  ) {    authorId    sport    location {      coordinates    }  }}");
        var result = await Query(@"
            query {
              places {
                name
              }
            }
            ");
        StringAssert.Contains("MUSZELKA", result);
    }

    [Test]
    public async Task DeletePlace()
    {
        await Query(
            "mutation {  createPlace(    input: {      id: 7      sport: \"swimming\"      latitude: 1      longitude: 2      name: \"Plywalnia KARPIK\"    }  ) {    authorId    sport    location {      coordinates    }  }}");
        var result = await Query(@"
            query {
              places {
                name
              }
            }
            ");
        StringAssert.Contains("KARPIK", result);

        await Query(@"
        mutation {
          deletePlace(id: 7) {
            name
          }
        }
        ");
        var result2 = await Query(@"
            query {
              places {
                name
              }
            }
            ");
        StringAssert.DoesNotContain("KARPIK", result2);
    }

    // [Test]
    // public async Task ImportPlaces()
    // {
    //     StringAssert.Contains("1131", await Query(@"
    //         mutation {
    //           importPlaces
    //         }
    //     "));
    // }
}