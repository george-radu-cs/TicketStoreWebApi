# Ticket Store Web API .NET 5

## Install dependencies

```console
dotnet add package Microsoft.EntityFrameworkCore.SqlServer -- version 5.0
dotnet tool install --global dotnet-ef -- version 5.0
dotnet add package Microsoft.EntityFrameworkCore.Design -- version 5.0
dotnet add package Microsoft.EntityFrameworkCore.Relational -- version 5.0
dotnet add package Microsoft.EntityFrameworkCore.Tools -- version 5.0
dotnet add package Microsoft.EntityFrameworkCore -- version 5.0
dotnet add package Microsoft.EntityFrameworkCore.Proxies -- version 5.0
dotnet add package Microsoft.AspNetCore.Mvc.NewtonsoftJson -- version 5.0
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 5.0
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 5.0
```

## Add Migrations

```console
dotnet ef migrations add <migration-name>
dotnet ef migrations remove
dotnet ef database update
```

## Insert Roles in DB

Some basic roles for app

```sql
INSERT INTO dbo.AspNetRoles ([Id], [Name], [NormalizedName], [ConcurrencyStamp])
VALUES (1, 'Admin', 'ADMIN', NULL)

INSERT INTO dbo.AspNetRoles ([Id], [Name], [NormalizedName], [ConcurrencyStamp])
VALUES (2, 'Buyer', 'BUYER', NULL)

INSERT INTO dbo.AspNetRoles ([Id], [Name], [NormalizedName], [ConcurrencyStamp])
VALUES (3, 'Organizer', 'ORGANIZER', NULL)
```

## TODO

Basic features:

 - [x] authentication
 - [ ] create all tables in db
 - [x] add authentication controller
 - [x] add event controller
 - [x] add reviews controller
 - [x] add ticket controller
 - [x] relation one-to-one
 - [x] relation one-to-many
 - [x] relation many-to-many
 - [x] add authorization for endpoints
 - [ ] input validations for all methods
 - [ ] add response models to now return all types of data to web
 - [ ] change returning https codes and error messages for all endpoints
 - [x] enhance password encryption by adding some random key from .env

To be added after ALL basic features are done and have more time left

- [ ] verify email at signup
- [ ] add reset password with mail verification - SendGrid
- [ ] add phone verification using twilio for one task above
- [ ] send receipt to user's mail when buying tickets from site - SendGrid
- [ ] add cron to update event status if it's to late to buy tickets (check daily)
 