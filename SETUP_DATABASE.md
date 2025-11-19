# Създаване на базата данни - Стъпка по стъпка

## Метод 1: Visual Studio Package Manager Console (НАЙ-ЛЕСНО) ⭐

1. **Отворете Visual Studio**
2. **Отворете Package Manager Console:**
   - Tools → NuGet Package Manager → Package Manager Console
   - Или: View → Other Windows → Package Manager Console

3. **Изпълнете следните команди една по една:**

```powershell
Add-Migration InitialCreate
```

След успешно създаване на миграциите:

```powershell
Update-Database
```

4. **Готово!** Базата данни е създадена.

---

## Метод 2: Command Prompt (след рестарт)

1. **Затворете всички Command Prompt прозорци**
2. **Отворете нов Command Prompt като администратор:**
   - Натиснете Windows + X
   - Изберете "Windows PowerShell (Admin)" или "Command Prompt (Admin)"

3. **Навигирайте до проекта:**
   ```cmd
   cd "C:\Users\marto\Desktop\Актьор Педерас"
   ```

4. **Изпълнете командите:**
   ```cmd
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

---

## Метод 3: Visual Studio - Solution Explorer

1. **Отворете Visual Studio**
2. **Кликнете с десен бутон на проекта** в Solution Explorer
3. **Изберете:** Open in Terminal
4. **В терминала изпълнете:**
   ```cmd
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

---

## Проверка дали работи

След създаване на базата данни:

1. **Рестартирайте приложението** (F5 в Visual Studio)
2. **Отидете на:** https://localhost:56187
3. **Влезте с админ акаунт:**
   - Имейл: `admin@hotel.com`
   - Парола: `Admin123!`

---

## Ако има проблеми

### Грешка: "dotnet-ef is not recognized"
**Решение:** Рестартирайте терминала или използвайте Visual Studio Package Manager Console

### Грешка: "Cannot open database"
**Решение:** Проверете дали SQL Server LocalDB е инсталиран:
```cmd
sqllocaldb info
```

### Грешка: "Login failed"
**Решение:** Променете connection string в `appsettings.json` или използвайте SQL Server Express

---

## Препоръка

**Използвайте Visual Studio Package Manager Console** - това е най-надеждният метод и работи винаги!

