# ====== Build Stage ======
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o /app/publish --no-restore

# ====== Runtime Stage ======
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
ENV ASPNETCORE_URLS=http://+:80
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Events system.dll"]

#docker rm -f tms-api-container tms-postgres


