@echo off
chcp 65001 >nul
echo ========================================
echo Създаване на база данни (Опростена версия)
echo ========================================
echo.

echo Стъпка 1: Инсталиране на Entity Framework Tools...
dotnet tool install --global dotnet-ef --version 8.0.0
echo.

echo Стъпка 2: Създаване на миграции...
dotnet ef migrations add InitialCreate
if %errorlevel% neq 0 (
    echo.
    echo ГРЕШКА: Неуспешно създаване на миграции!
    echo.
    pause
    exit /b 1
)
echo.

echo Стъпка 3: Създаване на базата данни...
dotnet ef database update
if %errorlevel% neq 0 (
    echo.
    echo ГРЕШКА: Неуспешно създаване на базата данни!
    echo.
    pause
    exit /b 1
)

echo.
echo ========================================
echo Базата данни е успешно създадена!
echo ========================================
echo.
pause

