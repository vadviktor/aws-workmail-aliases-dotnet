
# Appsettings

```powershell
op inject -f -i appsettings.json -o appsettings.Development.json
$env:DOTNET_ENVIRONMENT = "development"; dotnet run
```
