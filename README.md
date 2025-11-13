# ShortUrl – Minimal URL Shortener (ASP.NET Core .NET 9)


Tiny service that shortens URLs, stores them in SQLite with EF Core, and redirects while counting hits.


## Features
- POST `/shorten` → `{ code }`
- GET `/{code}` → 302 redirect
- GET `/stats/{code}` → usage info
- SQLite + EF Core migrations
- Swagger UI
- xUnit tests
- Dockerfile & CI

