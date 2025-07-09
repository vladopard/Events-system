# FitSync

FitSync is a workout and personal records tracker built with **ASP.NET Core 8** and a **React** front end. The repository contains a `Backend` API project and a `Frontend` single‑page application. When the API starts, it applies any pending EF Core migrations and seeds demo users.

## Features

- User registration & login (roles: admin/user)  
- Create, list and view events  
- Ticket purchase or join waiting queue when sold out  
- Admin dashboard for managing events, tickets & ticket types  
- Swagger UI for exploring & testing the API 

## Tech Stack

- **API**: ASP.NET Core 8, Entity Framework Core, PostgreSQL, ASP.NET Identity, JWT  
- **Frontend**: React, React Router, Axios, Nginx  
- **Cache/Session**: Redis  
- **Containerization**: Docker & Docker Compose, Git Hub Actions

## Getting Started

### Clone & Run
git clone https://github.com/<your‑username>/events-system.git
cd events-system
docker compose up --build

### Configuration
- Environment Variable	Purpose	Default
- POSTGRES_USER	Database superuser	postgres
- POSTGRES_PASSWORD	Database password	postgres
- POSTGRES_DB	Database name	EventsDb
- REDIS_HOST	Redis hostname (service name)	event_redis
- API__ConnectionStrings__DefaultConnection	EF Core connection string	see docker-compose.yml
