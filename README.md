# SportNiteServer - backend service for SportNite

## About
.NET Core-based API service exposing GraphQL interfaces for handling SportNite data

## Architecture
- written in ASP.NET Core (C#)
- data is persisted in MySQL database (connection by Entity Framework Core)
- packed into Docker container
- deployment through Github Actions into Kubernetes cluster on VPS
- integration tests for API (~65% coverage)
- uses https://open-meteo.com/ for weather forecast 
- uses http://overpass-turbo.eu/ data for exposing list of OpenStreetMap sport-related places
- Qodana code quality report: https://qodana.cloud/projects/3PE6p/reports/p4a7W

## GraphQL API
In order to explore GraphQL API you can use GraphQL Playground. "Docs" tab (on the left side) shows interactive API explorer.
Some queries/mutations have additional descriptions allowing developer better understanding.

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