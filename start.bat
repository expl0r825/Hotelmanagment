@echo off
chcp 65001 >nul
echo ========================================
echo Стартиране на Hotel Management System
echo ========================================
echo.

echo Стъпка 1: Възстановяване на пакетите...
call dotnet restore
if %errorlevel% neq 0 (
    echo ГРЕШКА: Неуспешно възстановяване на пакетите!
    pause
    exit /b 1
)

echo.
echo Стъпка 2: Компилиране на проекта...
call dotnet build
if %errorlevel% neq 0 (
    echo ГРЕШКА: Неуспешна компилация!
    pause
    exit /b 1
)

echo.
echo Стъпка 3: Проверка за Entity Framework Tools...
dotnet ef --version >nul 2>&1
if %errorlevel% neq 0 (
    echo Инсталиране на Entity Framework Tools...
    call dotnet tool install --global dotnet-ef
)

echo.
echo Стъпка 4: Проверка за миграции...
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
echo Стъпка 5: Обновяване на базата данни...
call dotnet ef database update
if %errorlevel% neq 0 (
    echo ГРЕШКА: Неуспешно обновяване на базата данни!
    pause
    exit /b 1
)

echo.
echo Стъпка 6: Стартиране на приложението...
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

