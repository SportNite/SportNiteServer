FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SportNiteServer/SportNiteServer.csproj", "SportNiteServer/"]
RUN dotnet restore "SportNiteServer/SportNiteServer.csproj"
COPY . .
WORKDIR "/src/SportNiteServer"
RUN dotnet build "SportNiteServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SportNiteServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY Assets /app/Assets
ENTRYPOINT ["dotnet", "SportNiteServer.dll"]
