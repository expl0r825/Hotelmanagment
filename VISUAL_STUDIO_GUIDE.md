# Ръководство за Visual Studio

## Решаване на грешката "Unable to start program"

Тази грешка се появява, когато проектът не е компилиран или липсват миграции. Ето как да я решите:

### Стъпка 1: Компилирайте проекта

1. В Visual Studio натиснете **Ctrl+Shift+B** или отидете на **Build → Build Solution**
2. Проверете дали има грешки в **Error List** (View → Error List)
3. Ако има грешки, поправете ги

### Стъпка 2: Създайте миграциите

1. Отворете **Package Manager Console** (Tools → NuGet Package Manager → Package Manager Console)
2. Изпълнете следните команди:

```powershell
Add-Migration InitialCreate
Update-Database
```

### Стъпка 3: Проверка на настройките

1. Проверете дали `appsettings.json` съдържа правилния connection string:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=HotelManagementDb;Trusted_Connection=True;MultipleActiveResultSets=true"
   }
   ```

2. Уверете се, че имате инсталиран SQL Server LocalDB

### Стъпка 4: Стартиране

1. Натиснете **F5** или кликнете на **Start** бутона
2. Приложението трябва да стартира на `https://localhost:56187`

## Алтернативен метод: Използване на Command Line

Ако имате проблеми с Visual Studio:

1. Отворете **Command Prompt** или **PowerShell**
2. Навигирайте до папката на проекта
3. Изпълнете `start.bat` файла (двоен клик)

## Админ достъп

След успешно стартиране:
- **Имейл:** admin@hotel.com
- **Парола:** Admin123!

## Често срещани проблеми

### "Cannot find the file specified"
- Уверете се, че сте компилирали проекта (Ctrl+Shift+B)
- Проверете дали имате създадени миграции

### "Database connection error"
- Уверете се, че SQL Server LocalDB е инсталиран
- Проверете connection string в `appsettings.json`

### "Migration not found"
- Изпълнете `Add-Migration InitialCreate` в Package Manager Console
- След това `Update-Database`

