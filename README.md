# Catering

An ASP.NET Core 8 MVC web application for managing a catering service. Supports user registration/login (including Google OAuth), role-based access (Admin, Caterer, Customer), a menu with item categories, and Stripe payment integration.

---

## Tech Stack

- **Framework:** ASP.NET Core 8 MVC
- **Database:** SQL Server (LocalDB by default) via Entity Framework Core
- **Auth:** ASP.NET Identity + Google OAuth
- **Payments:** Stripe.net
- **Language:** C# / Razor Views

---

## Prerequisites

Before running on a new machine, make sure you have:

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [SQL Server LocalDB](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb) — comes bundled with Visual Studio, or install separately via the SQL Server Express installer
- (Optional) [Visual Studio 2022](https://visualstudio.microsoft.com/) or VS Code with the C# extension

---

## Setup

### 1. Clone the repo

```bash
git clone https://github.com/TreeOfUselessWisdom/Catering.git
cd Catering
```

### 2. Configure the database connection

The default connection string in `appsettings.json` points to LocalDB:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=Catering;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

This works out of the box if you have LocalDB installed. If you want to use a different SQL Server instance, update `Server=` accordingly.

> **Note:** Migrations run automatically on startup (`db.Database.Migrate()`), so you don't need to run `dotnet ef database update` manually. The database and tables will be created on first launch.

### 3. Set up Google OAuth (required for Google login)

Go to [Google Cloud Console](https://console.cloud.google.com/), create a project, enable the OAuth 2.0 API, and create credentials. Then add your credentials to `appsettings.json`:

```json
"Authentication": {
  "Google": {
    "ClientId": "YOUR_CLIENT_ID_HERE",
    "ClientSecret": "YOUR_CLIENT_SECRET_HERE"
  }
}
```

Set the authorized redirect URI in Google Console to:
```
https://localhost:PORT/signin-google
```

> **Tip:** If you don't want Google login, the app will still run — just leave ClientId and ClientSecret blank. Email/password registration still works.

### 4. Set up Stripe (optional, for payment features)

Add your Stripe keys to `appsettings.json`:

```json
"Stripe": {
  "PublishableKey": "pk_test_...",
  "SecretKey": "sk_test_..."
}
```

If you don't have Stripe set up yet, payment-related features won't work but the rest of the app will.

### 5. Run the app

```bash
dotnet run
```

Or open `Catering.csproj` in Visual Studio and press F5.

The app will be available at `https://localhost:PORT` (check the terminal output for the exact port).

---

## What Happens on First Run

The app auto-seeds the following on startup if the database is empty:

**Roles:** Admin, Customer, Caterer

**Item Categories:**
- Buffet — Hot & cold buffet items
- Appetizers — Finger foods & starters
- Desserts — Cakes & sweets

**Sample Items:**
- Chicken Korma (Buffet) — Rs. 12.50
- Fries (Appetizers) — Rs. 5.00
- Chocolate Cake (Desserts) — Rs. 6.75

An **Admin account** is also seeded automatically via `IdentitySeed`. Check the `Data/IdentitySeed.cs` file for the default admin credentials if you've forgotten them.

---

## Project Structure

```
Catering/
├── Controllers/          # MVC controllers
├── Models/               # Entity models (Item, Order, etc.)
├── Views/                # Razor views
├── Data/                 # AppDbContext + seed logic
├── Migrations/           # EF Core migrations
├── Areas/Identity/       # Scaffolded Identity pages (login, register, etc.)
├── wwwroot/              # Static files (CSS, JS, images)
├── Program.cs            # App entry point and service configuration
├── appsettings.json      # Config (connection string, Google OAuth, etc.)
└── Catering.csproj       # Project file
```

---

## Notes

- **Email:** The app uses a `LocalEmailSender` that logs emails to `email_log.txt` in the project root instead of sending real emails. No SMTP setup needed.
- **Culture:** The app is set to `ur-PK` (Pakistani Rupee formatting). Prices display accordingly.
- **`bin/` and `obj/`** are committed to the repo (not ideal, but harmless — just ignore them).
