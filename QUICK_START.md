# Бърз старт

## Вариант 1: Използване на batch файла (Препоръчително)

Двоен клик на файла `start.bat` или изпълнете в терминал:
```cmd
start.bat
```

Това автоматично ще:
1. Възстанови пакетите
2. Компилира проекта
3. Създаде миграциите (ако липсват)
4. Създаде базата данни
5. Стартира приложението

## Вариант 2: Ръчно стартиране

### В Command Prompt или PowerShell:

1. **Възстановете пакетите:**
   ```cmd
   dotnet restore
   ```

2. **Компилирайте проекта:**
   ```cmd
   dotnet build
   ```

3. **Инсталирайте EF Tools (ако нямате):**
   ```cmd
   dotnet tool install --global dotnet-ef
   ```

4. **Създайте миграциите:**
   ```cmd
   dotnet ef migrations add InitialCreate
   ```

5. **Създайте базата данни:**
   ```cmd
   dotnet ef database update
   ```

6. **Стартирайте приложението:**
   ```cmd
   dotnet run
   ```

## Вариант 3: Visual Studio

1. Отворете `HotelManagement.csproj` в Visual Studio
2. Натиснете **Ctrl+Shift+B** за компилиране
3. Ако има грешки, изпълнете миграциите от Package Manager Console:
   ```
   Add-Migration InitialCreate
   Update-Database
   ```
4. Натиснете **F5** за стартиране

## Адрес на приложението

След стартиране, приложението ще бъде достъпно на:
- **HTTPS:** https://localhost:56187
- **HTTP:** http://localhost:56188

## Админ достъп

- **Имейл:** admin@hotel.com
- **Парола:** Admin123!

## Решаване на проблеми

### Грешка: "Unable to start program"
- Уверете се, че сте компилирали проекта (`dotnet build`)
- Проверете дали имате създадени миграции
- Изпълнете `start.bat` за автоматично настройване

### Грешка с базата данни
- Уверете се, че имате SQL Server LocalDB инсталиран
- Проверете connection string в `appsettings.json`

### Проблеми с кирилицата в пътя
- Използвайте `start.bat` файла
- Или преместете проекта в папка с латински имена

