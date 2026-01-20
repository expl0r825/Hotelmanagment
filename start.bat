@echo off
chcp 65001 >nul
echo ========================================
echo Стартиране на Hotel Management System
echo ========================================
echo.

echo Стъпка 1: Проверка за конфигурация...
if not exist "appsettings.Development.json" (
    if exist "appsettings.Development.json.example" (
        echo Създаване на appsettings.Development.json от пример...
        copy appsettings.Development.json.example appsettings.Development.json >nul
        echo Файлът е създаден успешно!
    ) else (
        echo ВНИМАНИЕ: appsettings.Development.json.example не е намерен!
    )
)
echo.

echo Стъпка 2: Възстановяване на пакетите...
call dotnet restore
if %errorlevel% neq 0 (
    echo ГРЕШКА: Неуспешно възстановяване на пакетите!
    pause
    exit /b 1
)

echo.
echo Стъпка 3: Компилиране на проекта...
call dotnet build
if %errorlevel% neq 0 (
    echo ГРЕШКА: Неуспешна компилация!
    pause
    exit /b 1
)

echo.
echo Стъпка 4: Проверка за Entity Framework Tools...
dotnet ef --version >nul 2>&1
if %errorlevel% neq 0 (
    echo Инсталиране на Entity Framework Tools...
    call dotnet tool install --global dotnet-ef
)

echo.
echo Стъпка 5: Проверка за миграции...
if not exist "Migrations" (
    echo Създаване на миграции...
    call dotnet ef migrations add InitialCreate
    if %errorlevel% neq 0 (
        echo ГРЕШКА: Неуспешно създаване на миграции!
        pause
        exit /b 1
    )
)

echo.
echo Стъпка 6: Обновяване на базата данни...
call dotnet ef database update
if %errorlevel% neq 0 (
    echo ГРЕШКА: Неуспешно обновяване на базата данни!
    pause
    exit /b 1
)

echo.
echo Стъпка 7: Стартиране на приложението...
echo.
echo ========================================
echo Приложението ще стартира на:
echo https://localhost:56187
echo http://localhost:56188
echo ========================================
echo.
echo Админ достъп:
echo Имейл: admin@hotel.com
echo Парола: Admin123!
echo.
echo Натиснете Ctrl+C за спиране на приложението
echo.

call dotnet run

pause

