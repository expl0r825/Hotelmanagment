# Ръководство за настройка на проекта

## Първоначална настройка след изтегляне от GitHub

### Стъпка 1: Клониране/Изтегляне на проекта

```bash
git clone <repository-url>
cd HotelManagement
```

### Стъпка 2: Настройка на конфигурацията

1. **Копирайте примерните конфигурационни файлове:**

   ```bash
   # Windows (PowerShell)
   Copy-Item appsettings.Development.json.example appsettings.Development.json
   
   # Windows (CMD)
   copy appsettings.Development.json.example appsettings.Development.json
   
   # Linux/Mac
   cp appsettings.Development.json.example appsettings.Development.json
   ```

2. **Редактирайте `appsettings.Development.json`** (ако е необходимо):
   - По подразбиране използва SQL Server LocalDB
   - Ако използвате друг SQL Server, променете connection string

### Стъпка 3: Възстановяване на пакетите

```bash
dotnet restore
```

### Стъпка 4: Създаване на базата данни

#### Вариант A: Използване на Visual Studio
1. Отворете проекта в Visual Studio
2. Tools → NuGet Package Manager → Package Manager Console
3. Изпълнете:
   ```powershell
   Add-Migration InitialCreate
   Update-Database
   ```

#### Вариант B: Използване на Command Line
```bash
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Стъпка 5: Стартиране на приложението

```bash
dotnet run
```

Или в Visual Studio: **F5**

## Настройка на базата данни

### SQL Server LocalDB (Препоръчително за разработка)

Connection string в `appsettings.Development.json`:
```json
"DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=HotelManagementDb;Trusted_Connection=True;MultipleActiveResultSets=true"
```

### SQL Server Express

```json
"DefaultConnection": "Server=.\\SQLEXPRESS;Database=HotelManagementDb;Trusted_Connection=True;MultipleActiveResultSets=true"
```

### SQL Server (Production)

```json
"DefaultConnection": "Server=YOUR_SERVER;Database=HotelManagementDb;User Id=YOUR_USER;Password=YOUR_PASSWORD;Trusted_Connection=False;MultipleActiveResultSets=true"
```

## Първоначални данни

При първото стартиране на приложението автоматично се създават:
- **Админ потребител:**
  - Имейл: `admin@hotel.com`
  - Парола: `Admin123!`
- **5 примерни стаи** с локални снимки

## Структура на папките

```
HotelManagement/
├── wwwroot/
│   └── images/          # Статични снимки на стаи
│       ├── room-single.png
│       ├── room-double.png
│       ├── room-suite.png
│       └── room-family.png
├── Migrations/          # EF Core миграции (създават се автоматично)
├── appsettings.json     # Основна конфигурация (не се променя)
└── appsettings.Development.json  # Локална конфигурация (не се комитира)
```

## Често срещани проблеми

### Проблем: "Cannot open database"
**Решение:** Уверете се, че SQL Server LocalDB е инсталиран и че сте изпълнили миграциите.

### Проблем: "dotnet-ef is not recognized"
**Решение:** 
```bash
dotnet tool install --global dotnet-ef
```

### Проблем: Снимките не се показват
**Решение:** Уверете се, че снимките са в `wwwroot/images/` и че имената съвпадат с тези в SeedData.

## Порт на приложението

По подразбиране приложението стартира на:
- HTTPS: `https://localhost:56187`
- HTTP: `http://localhost:56188`

Можете да промените портовете в `Properties/launchSettings.json`.

