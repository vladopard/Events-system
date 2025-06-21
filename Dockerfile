# ====== Build Stage ======
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копирамо само .csproj прво – ово се ретко мења, па ће се кеширати
COPY *.csproj ./
RUN dotnet restore

# Сада тек остатак пројекта – ако мењаш само .cs фајл, ово ће бити једини слој који се поново ради
COPY . ./
RUN dotnet build -c Release --no-restore   # додатно кеширање пре publish-а
RUN dotnet publish -c Release -o /app/publish --no-restore

# ====== Runtime Stage ======
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
ENV ASPNETCORE_URLS=http://+:80
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Events system.dll"]
