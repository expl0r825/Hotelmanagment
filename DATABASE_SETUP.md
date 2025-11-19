# Настройка на базата данни

## Проблем: "Cannot open database HotelManagementDb"

Тази грешка означава, че базата данни не е създадена. Ето как да я решите:

## Решение 1: Използване на batch файла (Препоръчително)

1. **Двоен клик на `create_database.bat`**
2. Скриптът автоматично ще:
   - Инсталира Entity Framework Tools (ако липсват)
   - Създаде миграциите
   - Създаде базата данни

## Решение 2: Ръчно в Visual Studio

### Стъпка 1: Отворете Package Manager Console
- **Tools → NuGet Package Manager → Package Manager Console**

### Стъпка 2: Създайте миграциите
```powershell
Add-Migration InitialCreate
```

### Стъпка 3: Създайте базата данни
```powershell
Update-Database
```

## Решение 3: Ръчно в Command Prompt

1. Отворете **Command Prompt** или **PowerShell**
2. Навигирайте до папката на проекта
3. Изпълнете:

```cmd
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## Проверка на SQL Server LocalDB

Ако имате проблеми, проверете дали SQL Server LocalDB е инсталиран:

1. Отворете **Command Prompt**
2. Изпълнете:
```cmd
sqllocaldb info
```

Ако видите списък с инстанции, LocalDB е инсталиран.

## Алтернативен Connection String

Ако LocalDB не работи, можете да използвате SQL Server Express. Променете `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.\\SQLEXPRESS;Database=HotelManagementDb;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

Или използвайте SQLite (за тестване):

```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=HotelManagement.db"
}
```

И променете в `Program.cs`:
```csharp
options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
```

## След успешно създаване

След като базата данни е създадена:
1. Рестартирайте приложението
2. При първото стартиране ще се създаде админ потребител автоматично
3. Влезте с:
   - **Имейл:** admin@hotel.com
   - **Парола:** Admin123!

