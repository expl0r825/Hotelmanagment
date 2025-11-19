@echo off
chcp 65001 >nul
echo ========================================
echo Създаване на база данни
echo ========================================
echo.

echo Стъпка 1: Проверка за Entity Framework Tools...
dotnet ef --version >nul 2>&1
if %errorlevel% neq 0 (
    echo Инсталиране на Entity Framework Tools...
    dotnet tool install --global dotnet-ef --version 8.0.0
    REM Проверка дали инсталацията е успешна чрез проверка на изхода
    dotnet ef --version >nul 2>&1
    if %errorlevel% neq 0 (
        echo.
        echo ВНИМАНИЕ: EF Tools може да не са достъпни в текущата сесия.
        echo Моля, затворете и отворете отново Command Prompt, след което изпълнете:
        echo   dotnet ef migrations add InitialCreate
        echo   dotnet ef database update
        echo.
        pause
        exit /b 1
    )
    echo.
    echo Успешно инсталирани!
    echo.
) else (
    echo EF Tools вече са инсталирани.
    echo.
)

echo Стъпка 2: Проверка за съществуващи миграции...
if exist "Migrations" (
    echo Намерени са съществуващи миграции.
    echo.
    echo Стъпка 3: Обновяване на базата данни...
    call dotnet ef database update
    if %errorlevel% neq 0 (
        echo.
        echo ГРЕШКА: Неуспешно обновяване на базата данни!
        echo.
        echo Опит за изтриване на старите миграции и създаване на нови...
        echo.
        pause
        goto :create_new
    )
) else (
    echo Няма налични миграции.
    echo.
    goto :create_new
)

echo.
echo ========================================
echo Базата данни е успешно създадена!
echo ========================================
echo.
pause
exit /b 0

:create_new
echo Стъпка 3: Създаване на миграции...
call dotnet ef migrations add InitialCreate
if %errorlevel% neq 0 (
    echo.
    echo ГРЕШКА: Неуспешно създаване на миграции!
    echo.
    echo Възможни причини:
    echo 1. SQL Server LocalDB не е инсталиран
    echo 2. Connection string е неправилен
    echo 3. Няма права за достъп до базата данни
    echo.
    pause
    exit /b 1
)

echo.
echo Стъпка 4: Създаване на базата данни...
call dotnet ef database update
if %errorlevel% neq 0 (
    echo.
    echo ГРЕШКА: Неуспешно създаване на базата данни!
    echo.
    echo Възможни причини:
    echo 1. SQL Server LocalDB не е инсталиран
    echo 2. Connection string е неправилен
    echo 3. Няма права за достъп до базата данни
    echo.
    pause
    exit /b 1
)

echo.
echo ========================================
echo Базата данни е успешно създадена!
echo ========================================
echo.
echo Сега можете да стартирате приложението.
echo.
pause

