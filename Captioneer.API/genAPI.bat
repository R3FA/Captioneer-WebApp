@echo off
dotnet new sln --name Captioneer.API
dotnet sln add API
dotnet sln add UtilityService
pause