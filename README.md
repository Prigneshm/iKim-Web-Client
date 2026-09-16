# i'Kim Web Console (IKimWebConsole)

ASP.NET MVC 5 web client/admin console for **i'Kim** — a stock, item, and order fulfillment management system. The app itself holds no business logic against a database; it is a browser-based front end that authenticates users and talks to a separate i'Kim backend REST API over HTTP.

## Overview

The console provides screens for:

- **Authentication** — login, logout, forgot password, change password
- **Dashboard** — landing page after login
- **Catalog management** — Categories, Items, Pricing Tiers, Units of Measure
- **Stock management** — Stock and Stock Items
- **Store management** — Stores, Store Types, Addresses
- **Order management** — Orders and Order Lines
- **User management** — Users, User Types
- **Fulfillment Log** — order fulfillment history

Requests to the backend go through a shared `HttpClientService`, with the authenticated user's session tracked via a cookie (`IKimLoginUser`) and JSON payloads handled with Newtonsoft.Json.

## Solution structure

| Project | Purpose |
|---|---|
| `IKimWebConsole` | The MVC 5 web application — controllers, Razor views, static assets, and app configuration |
| `IKimWebConsole.Domain` | POCO models shared across the app (`Item`, `Order`, `Store`, `User`, `Stock`, etc.) and their `*Lister` list/paging variants |
| `IKimWebConsole.Infrastructure` | Cross-cutting concerns: service interfaces (`IService`), the HTTP client abstraction, exceptions, enums, and helpers |
| `IKimWebConsole.Service` | Concrete service implementations that call the backend API on behalf of the controllers |

Dependency injection is wired up with **Unity** (`Unity.Mvc5`) in `App_Start/UnityConfig.cs`.

## Tech stack

- ASP.NET MVC 5 / Razor, targeting **.NET Framework 4.8**
- Unity for dependency injection
- Bootstrap 5.2.3, jQuery 3.7, jQuery Validation, jQuery Unobtrusive Validation
- Newtonsoft.Json for API serialization
- ASP.NET Web Optimization (bundling/minification)

## Prerequisites

- Visual Studio 2022 (or later) with the **ASP.NET and web development** workload
- .NET Framework 4.8 Developer Pack
- IIS Express (bundled with Visual Studio) or IIS
- NuGet package restore enabled
- A running instance of the i'Kim backend API that this console will call

## Getting started

1. Clone the repository:
   ```bash
   git clone https://github.com/Prigneshm/iKim-Web-Client.git
   ```
2. Open `IKimWebConsole.sln` in Visual Studio and let it restore the NuGet packages listed in each project's `packages.config`.
3. Point the app at your backend API by setting `APIBaseUrl` (see **Configuration** below).
4. Set `IKimWebConsole` as the startup project and run (F5) — this launches the app under IIS Express. The default route (`Authentication/Index`) is the login page.

## Configuration

The backend API endpoint is controlled by the `APIBaseUrl` app setting, e.g. in `IKimWebConsole/Config/App.Dev.config`:

```xml
<add key="APIBaseUrl" value="https://localhost:44342/api" />
```

Update this to point at the environment you want the console to talk to. Standard ASP.NET Web.config transforms are used for environment-specific overrides:

- `Web.Debug.config` — Debug build transform
- `Web.Release.config` — Release build transform (strips `debug` from `<compilation>`)

No database connection strings are configured in this project, since all data access happens through the backend API rather than directly against a database.

## Notes

- This repository does not currently define a license; treat it as private/proprietary unless stated otherwise.
