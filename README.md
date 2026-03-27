# Smart Event Management and Ticketing System
## Metropolitan Cultural Council - Coursework

### Project Structure

```
Web/
├── Database/               # Database design and scripts
│   ├── Schema.sql         # CREATE TABLE (SQL Server)
│   ├── SampleData.sql     # INSERT sample data
│   ├── Queries.sql        # SELECT examples
│   ├── DataDictionary.md  # Attribute specifications
│   └── ER-Diagram.md      # ER diagram (Mermaid)
├── EventManagementSystem/ # ASP.NET Core MVC application
│   ├── Controllers/
│   ├── Models/
│   ├── Views/
│   ├── Services/
│   ├── Data/
│   └── ...
├── Documentation/         # Report and documentation
│   ├── README.md         # Contents page
│   └── FULL_REPORT.md    # Full design & implementation report
├── Tests/                 # Test scenarios
│   └── TestScenarios.md
└── README.md
```

### Quick Start

1. **Restore packages**
   ```bash
   cd EventManagementSystem
   dotnet restore
   ```

2. **Run the application**
   ```bash
   dotnet run
   ```

3. **Open browser**: https://localhost:5000 or http://localhost:5000

4. **Test accounts** (after first run with SQLite):
   - Email: `john@test.com` | Password: `Member123!`
   - Email: `jane@test.com` | Password: `Member123!`

### Database

- **Development**: Uses SQLite (`events.db`) by default. Sample data is seeded automatically.
- **SQL Server**: Set `ConnectionStrings:DefaultConnection` in `appsettings.json` to use SQL Server. Run `Database/Schema.sql` and `Database/SampleData.sql` manually, or let EF create the schema (EnsureCreated).

### Features Implemented

| Feature | Guests | Members |
|---------|--------|---------|
| Home & event overview | ✓ | ✓ |
| Browse events | ✓ (basic) | ✓ (full) |
| Search (category, date, location, price) | ✓ | ✓ |
| Availability (Available/Full) | ✓ | - |
| Seat details | - | ✓ |
| Book tickets | - | ✓ |
| Submit reviews | - | ✓ |
| Read reviews | ✓ | ✓ |
| Register | ✓ | ✓ |
| Send inquiry | ✓ | ✓ |
