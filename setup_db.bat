@echo off
chcp 65001
cls
echo.
echo ========================================
echo   СЪЗДАВАНЕ НА БАЗА ДАННИ
echo ========================================
echo.

REM Проверка дали сме в правилната директория
if not exist "HotelManagement.csproj" (
    echo ГРЕШКА: Не сте в папката на проекта!
    echo Моля, навигирайте до папката, където е HotelManagement.csproj
    echo.
    pause
    exit /b 1
)

echo [1/3] Проверка за dotnet-ef...
where dotnet-ef >nul 2>&1
if %errorlevel% neq 0 (
    echo.
    echo dotnet-ef не е намерен. Инсталиране...
    dotnet tool install --global dotnet-ef --version 8.0.0
    echo.
    echo ВАЖНО: След инсталация, моля затворете и отворете отново този прозорец!
    echo След това изпълнете отново този файл.
    echo.
    pause
    exit /b 0
)

echo [OK] dotnet-ef е наличен
echo.

echo [2/3] Създаване на миграции...
if exist "Migrations" (
    echo Миграциите вече съществуват. Пропускане...
) else (
    dotnet ef migrations add InitialCreate
    if %errorlevel% neq 0 (
        echo.
        echo ГРЕШКА при създаване на миграции!
        echo.
        pause
        exit /b 1
    )
    echo [OK] Миграциите са създадени
)
echo.

echo [3/3] Създаване на базата данни...
dotnet ef database update
if %errorlevel% neq 0 (
    echo.
    echo ГРЕШКА при създаване на базата данни!
    echo.
    echo Възможни причини:
    echo - SQL Server LocalDB не е инсталиран
    echo - Connection string е неправилен
    echo.
    pause
    exit /b 1
)

echo.
echo ========================================
echo   УСПЕХ! Базата данни е създадена!
echo ========================================
echo.
echo Сега можете да стартирате приложението.
echo.
pause

