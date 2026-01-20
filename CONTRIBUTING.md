# Ръководство за сътрудничество

## Настройка на средата за разработка

### Изисквания

- .NET 8.0 SDK или по-нова версия
- SQL Server LocalDB (за разработка) или SQL Server Express
- Visual Studio 2022 или Visual Studio Code (опционално)
- Git

### Стъпка 1: Клониране на проекта

```bash
git clone <repository-url>
cd HotelManagement
```

### Стъпка 2: Настройка на конфигурацията

1. Копирайте примерния конфигурационен файл:
   ```bash
   copy appsettings.Development.json.example appsettings.Development.json
   ```

2. Редактирайте `appsettings.Development.json` според вашите нужди:
   - По подразбиране използва SQL Server LocalDB
   - Ако използвате друг SQL Server, променете connection string

### Стъпка 3: Възстановяване на зависимости

```bash
dotnet restore
```

### Стъпка 4: Създаване на базата данни

```bash
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Стъпка 5: Стартиране на приложението

```bash
dotnet run
```

Приложението ще стартира на `https://localhost:56187`

## Структура на проекта

```
HotelManagement/
├── Controllers/         # MVC контролери
├── Models/              # Модели на данни и ViewModels
├── Views/               # Razor изгледи
├── Data/                # DbContext и SeedData
├── wwwroot/             # Статични файлове
│   ├── css/
│   ├── js/
│   └── images/          # Снимки на стаи
├── Migrations/          # EF Core миграции
└── Properties/          # Конфигурация на проекта
```

## Правила за комитиране

- Използвайте описателни commit съобщения
- Не комитирайте `appsettings.Development.json` или други чувствителни файлове
- Преди комит, проверете дали проектът се компилира успешно
- Тествайте функционалността преди комит

## Файлове, които НЕ трябва да се комитират

- `appsettings.Development.json`
- `appsettings.Production.json`
- `bin/` и `obj/` папки
- `.vs/` папка
- `wwwroot/images/rooms/` (качени от админ панела)

## Тестване

Преди да направите Pull Request:
1. Уверете се, че проектът се компилира без грешки
2. Тествайте основните функционалности
3. Проверете дали базата данни се създава правилно

## Въпроси и поддръжка

Ако имате въпроси или проблеми, моля отворете Issue в GitHub repository.

