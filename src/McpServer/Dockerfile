FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app

COPY McpServer.csproj ./
RUN dotnet restore McpServer.csproj

COPY . ./
RUN dotnet build McpServer.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish McpServer.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "McpServer.dll"]