dotnet build Tseesecake.sln -c Release --nologo 
dotnet test -c Release /p:CollectCoverage=true /p:CoverletOutputFormat=opencover --no-build --nologo
dotnet reportgenerator "-reports:Tseesecake.Server.Testing\coverage.*.opencover.xml" "-targetdir:.\.coverage"
start .coverage/index.html