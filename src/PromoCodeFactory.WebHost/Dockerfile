FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Копируем весь солюшен
COPY . ./
RUN dotnet restore PromoCodeFactory.WebHost/PromoCodeFactory.WebHost.csproj
RUN dotnet publish PromoCodeFactory.WebHost/PromoCodeFactory.WebHost.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "PromoCodeFactory.WebHost.dll"]