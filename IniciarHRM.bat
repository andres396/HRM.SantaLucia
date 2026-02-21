@echo off
title Sistema HRM - Santa Lucia
echo.
echo  Iniciando Sistema HRM Santa Lucia...
echo  Se abrira el navegador en unos segundos.
echo  Si no abre, visite: http://localhost:5000
echo.

REM Usar puerto fijo para poder abrir el navegador
set ASPNETCORE_URLS=http://localhost:5000

start "" "HRM.SantaLucia.Web.exe"

timeout /t 5 /nobreak >nul
start http://localhost:5000

echo.
echo  La aplicacion esta corriendo. Cierre esta ventana para detener el servidor.
pause
