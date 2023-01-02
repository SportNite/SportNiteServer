# SportNiteServer - backend service for SportNite

## About
.NET Core-based API service exposing GraphQL interfaces for handling SportNite data

## Specification
- written in ASP.NET Core (C#)
- data is persisted in MySQL database (connection by Entity Framework Core)
- packed into Docker container
- deployment through Github Actions into Kubernetes cluster on VPS
- integration tests for API (~65% coverage)
- uses https://open-meteo.com/ for weather forecast 
- uses http://overpass-turbo.eu/ data for exposing list of OpenStreetMap sport-related places
- Qodana code quality report: https://qodana.cloud/projects/3PE6p/reports/p4a7W

## Architecture
App is based on MVC pattern. It consists of 3 layers:
- Presentation (aka View) - responsible for handling queries and mutations (`Data` module)
- Services (aka Controllers) - responsible for business logic (`Services` module)
- Model - responsible for data access (`Entites` and `Database` modules)

## Code organization
#### `SportNiteServer` - main project with API and GraphQL interfaces
- `Program.cs` - starting point for entire service. Dependency injection setup, GraphQL schema setup, etc.
- `Dockerfile` - Dockerfile for building container
- `docker-compose.yml` - docker-compose file for local development
- `appsettings.json` - configuration file for local development
- `Services` - module containing business logic code for handling GraphQL queries and mutations
- `Entites` - module containing database entities
- `Database` - module containing database connection setup
- `Migrations` - module containing database migrations
- `Dto` - module containing `DTO`s for GraphQL queries and mutations (`DTO` stands for `Data Transfer Object`) 
- `Assets` - module containing data for seeding database
- `Data` - module containing `Query` and `Mutation` classes for GraphQL schema
- `Exceptions` - module containing custom exceptions and exception handlers

#### `SportNiteServer.Tests` - project containing integration tests for API
- `QueryIntegrationTests.cs` - integration tests for GraphQL queries
- `MutationIntegrationTests.cs` - integration tests for GraphQL mutations
- `UtilsUnitTests.cs` - unit tests for `Services.Utils`

### Interfaces

- `Database/DatabaseContext.cs` - database context for Entity Framework Core
- `Services/AuthService.cs` - service for handling authentication
- `Services/OfferService.cs` - service for handling offers CRUD
- `Services/PlaceService.cs` - service for handling places CRUD and seeding from file
- `Services/WeatherService.cs` - service for requesting weather forecast from https://open-meteo.com/ 
- `Services/ResponseService.cs` - service for handling responses CRUD, accepting or declining it
- `Services/UserService.cs` - service for handling users management
- `Services/Utils.cs` - contains utility functions

## GraphQL API
In order to explore GraphQL API you can use GraphQL Playground. "Docs" tab (on the right side) shows interactive API explorer.
Some queries/mutations have additional descriptions for better understanding by the developer.

![graphql_playground.png](screenshots/graphql_playground.png)

## Building and running

Easiest way to launch service is to utilize docker-compose setup:
```bash
git clone https://github.com/SportNite/SportNiteServer.git
cd SportNiteServer
docker-compose up -d --build
```
After starting up both services (database and server) you can access GraphQL Playground at http://localhost:7150/playground.

## Testing

Assuming dotnet core toolchain is installed and MySQL database is up and running (with empty database 'sportnite' created):

```bash
git clone https://github.com/SportNite/SportNiteServer.git
cd SportNiteServer
dotnet test
```

If you want to change database for testing, create `SportNiteServer/.env` file with following content:

```env
MYSQL_CONNECTION="server=somehost ; port=3306 ; database=somedatabase ; user=someuser ; password=somepassword"
```

## Code quality report
With `qodana` installed:
```bash
qodana scan --show-report
```

![qodana.png](screenshots/qodana.png)

## License
Project is licensed under the BSD 3-clause license. See [LICENSE](LICENSE) for more details.
Dependencies licenses are listed in [dependencies-licenses](dependencies-licenses.txt) file.
![deps.png](screenshots/deps.png)