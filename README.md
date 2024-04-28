# Website
The website of DevToys

# Update the documentation

1. Build the documentation website from the submodule/

```powershell
git submodule update --init --recursive
git submodule update --remote --merge
dotnet tool update -g docfx
docfx ".\submodule\Documentation\docfx.json"
```

2. Copy the documentation website to the `wwwroot` folder

```powershell
# Remove the 'doc' directory if it exists
Remove-Item -Path ".\src\Website\wwwroot\doc" -Recurse -Force

# Clear the 'submodule' directory in the _site folder.
Remove-Item -Path ".\submodule\Documentation\_site\submodule" -Recurse -Force

# Copy the documentation website to the 'doc' directory
Copy-Item -Path ".\submodule\Documentation\_site" -Destination ".\src\Website\wwwroot\doc" -Recurse -Force
```

3. Build and publish `Website.sln`.