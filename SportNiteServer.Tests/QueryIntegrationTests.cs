using System;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Execution;
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

        var schema = await serviceCollection.AddGraphQLServer()
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
        var request = QueryRequestBuilder.New()
            .SetQuery(query)
            .SetServices(_serviceProvider)
            .Create();
        var result = await _executor.ExecuteAsync(request);
        var output = await result.ToJsonAsync();
        return output;
    }

    [Test]
    public async Task GetVersion()
    {
        StringAssert.Contains("1.0.0", await Query("{version}"));
    }
}