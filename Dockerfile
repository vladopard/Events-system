# === build stage ======================================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# 1) kopiraj CSPROJ pa restore
COPY ["Events system.csproj", "./"]
RUN dotnet restore "./Events system.csproj"

# 2) kopiraj ostatak source-a
COPY . .
RUN dotnet publish "./Events system.csproj" -c Release -o /out

# === runtime stage ===================================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0
ENV ASPNETCORE_URLS=http://+:80
WORKDIR /app
COPY --from=build /out .
ENTRYPOINT ["dotnet", "Events system.dll"]
