# Решаване на Build Failed грешки

## Често срещани проблеми и решения

### Проблем 1: Липсващи using statements

Ако виждате грешки като:
- `The name 'Path' does not exist`
- `The name 'Directory' does not exist`
- `The name 'IWebHostEnvironment' does not exist`

**Решение:** Добавете липсващите using statements в началото на файла.

### Проблем 2: IWebHostEnvironment не се разпознава

В .NET 8.0, `IWebHostEnvironment` е в namespace `Microsoft.AspNetCore.Hosting`.

**Решение:** Добавете в началото на AdminController.cs:
```csharp
using Microsoft.AspNetCore.Hosting;
```

### Проблем 3: IFormFile не се разпознава

**Решение:** Добавете:
```csharp
using Microsoft.AspNetCore.Http;
```

### Проблем 4: System.Linq методи не работят

С ImplicitUsings в .NET 8.0, System.Linq трябва да е автоматично включен. Ако не работи, добавете:
```csharp
using System.Linq;
```

## Стъпки за отстраняване на грешки

1. **Отворете Error List в Visual Studio:**
   - View → Error List (или Ctrl+W, E)
   - Вижте всички грешки

2. **Проверете всяка грешка:**
   - Кликнете на грешката, за да отидете до файла
   - Вижте какво липсва

3. **Често срещани грешки:**

   **"The type or namespace name 'X' could not be found"**
   - Добавете липсващия using statement

   **"'X' does not exist in the current context"**
   - Проверете дали класът/методът е правилно написан
   - Проверете дали имате правилните using statements

4. **След поправяне на грешките:**
   - Натиснете Ctrl+Shift+B за компилиране
   - Проверете дали всички грешки са отстранени

## Автоматична проверка

Изпълнете `build_and_check.bat` за да видите всички грешки наведнъж.

## Ако проблемът продължава

1. Затворете Visual Studio
2. Изтрийте папките `bin` и `obj`
3. Отворете Visual Studio отново
4. Възстановете пакетите: `dotnet restore`
5. Компилирайте: `dotnet build`

